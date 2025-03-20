using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;
using System.CodeDom.Compiler;

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

        long? ArsenalID = null;

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

            // Check to see if user already has an arsenal or not (in the future the user will provide a specific arsenal to add to)
            string arsenalCheck = @"
            SELECT a.arsenalID
            FROM Arsenal AS a
            WHERE userID = @UserID
            ";
            using var arsenalCheckCommand = new SqlCommand(arsenalCheck, connection, transaction);
            arsenalCheckCommand.Parameters.AddWithValue("@UserID", userId);
            // Execute the query
            using SqlDataReader reader = await arsenalCheckCommand.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // User arsenal exists. Retrieve the column
                ArsenalID = Convert.ToInt64(reader["arsenalID"]);
                await reader.CloseAsync();
            }
            else
            {
                await reader.CloseAsync();
                // User arsenal does not exist create it
                string arsenalCreate = @"
                INSERT INTO Arsenal (userid, status)
                VALUES (@UserId, 1)
                SELECT SCOPE_IDENTITY();
                ";

                using var arsenalCreateCommand = new SqlCommand(arsenalCreate, connection, transaction);
                arsenalCreateCommand.Parameters.AddWithValue("@UserId", userId);

                // Execute the command and retrieve the newly inserted arsenalID
                var insertedArsenalId = await arsenalCreateCommand.ExecuteScalarAsync();

                if (insertedArsenalId == null || insertedArsenalId == DBNull.Value)
                {
                    throw new InvalidOperationException("Failed to insert arsenal or retrieve arsenal ID.");
                }
                ArsenalID = Convert.ToInt64(insertedArsenalId);
            }
            // Undelete a ball
            string selectQuery = @"
            UPDATE Ball
            SET status = 1
            WHERE status = 0
            AND ballid IN (
                SELECT b.ballid
                FROM [User] AS u
                INNER JOIN Arsenal AS a ON u.id = a.userid
                INNER JOIN Ball AS b ON @ArsenalID = b.ArsenalID
                WHERE u.username = @Username AND b.name = @BallName
            );";
            using var command = new SqlCommand(selectQuery, connection, transaction);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@BallName", ball.Name);
            command.Parameters.AddWithValue("@ArsenalID", ArsenalID);
            int success = await command.ExecuteNonQueryAsync();
            // If the ball existed, return true because it is now undeleted
            if (success > 0)
            {
                await transaction.CommitAsync();
                return true;
            }

            // Insert Ball and retrieve Ball ID
            string insertBallQuery = @"
            INSERT INTO [Ball] (name, diameter, weight, core_type, ArsenalID, status) 
            OUTPUT INSERTED.ballid 
            VALUES (@name, @diameter, @weight, @coretype, @ArsenalID, 1)";

            using var ballCommand = new SqlCommand(insertBallQuery, connection, transaction);
            ballCommand.Parameters.AddWithValue("@name", ball.Name ?? string.Empty);
            ballCommand.Parameters.AddWithValue("@diameter", ball.Diameter);
            ballCommand.Parameters.AddWithValue("@weight", ball.Weight);
            ballCommand.Parameters.AddWithValue("@coretype", ball.CoreType ?? string.Empty);
            ballCommand.Parameters.AddWithValue("@ArsenalID", ArsenalID);

            object? result = await ballCommand.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert ball or retrieve ball ID.");
            }

            int ballId = Convert.ToInt32(result);

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