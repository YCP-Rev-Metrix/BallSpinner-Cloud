using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddBall(Ball ball, string username)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new InvalidOperationException("Connection string is not set.");
        }

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            // Validate and fetch user ID
            int userId = await GetUserId(username);
            if (userId <= 0)
            {
                LogWriter.LogError($"Invalid user ID for username: {username}");
                return false;
            }

            string selectQuery = @"
            UPDATE Arsenal
            SET status = 1
            WHERE status = 0
            AND ball_id IN (
                SELECT b.ballid
                FROM [User] AS u
                INNER JOIN Arsenal AS a ON u.id = a.userid
                INNER JOIN Ball AS b ON a.ball_id = b.ballid
                WHERE u.username = @Username AND b.name = @BallName
            );";
            using var command = new SqlCommand(selectQuery, connection, transaction);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@BallName", ball.Name);
            int success = await command.ExecuteNonQueryAsync();
            if (success > 0)
            {
                await transaction.CommitAsync();
                return true;
            }

            // Insert Ball and retrieve Ball ID
            string insertBallQuery = @"
            INSERT INTO [Ball] (name, diameter, weight, core_type) 
            OUTPUT INSERTED.ballid 
            VALUES (@name, @diameter, @weight, @coretype)";

            using var ballCommand = new SqlCommand(insertBallQuery, connection, transaction);
            ballCommand.Parameters.AddWithValue("@name", ball.Name ?? string.Empty);
            ballCommand.Parameters.AddWithValue("@diameter", ball.Diameter);
            ballCommand.Parameters.AddWithValue("@weight", ball.Weight);
            ballCommand.Parameters.AddWithValue("@coretype", ball.CoreType ?? string.Empty);

            object? result = await ballCommand.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert ball or retrieve ball ID.");
            }

            int ballId = Convert.ToInt32(result);

            // Insert into Arsenal
            string insertArsenalQuery = @"
            INSERT INTO [Arsenal] (userid, ball_id, status) 
            VALUES (@userid, @ballid, @status)";

            using var arsenalCommand = new SqlCommand(insertArsenalQuery, connection, transaction);
            arsenalCommand.Parameters.AddWithValue("@userid", userId);
            arsenalCommand.Parameters.AddWithValue("@ballid", ballId);
            //Set status to 1 (active)
            arsenalCommand.Parameters.AddWithValue("@status", 1); 

            int rowsAffected = await arsenalCommand.ExecuteNonQueryAsync();
            if (rowsAffected <= 0)
            {
                throw new InvalidOperationException("Failed to associate ball with user in Arsenal.");
            }

            // Commit transaction
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            LogWriter.LogError($"Error occurred while adding a ball for user '{username}': {ex.Message}\n{ex}");
            await transaction.RollbackAsync();
            throw;
        }
    }

}