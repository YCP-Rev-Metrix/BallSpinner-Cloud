using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddBall(Ball ball, string username)
{
    // Look into user table, get id that matches with username
    using var connection = new SqlConnection(ConnectionString);
    await connection.OpenAsync();

    using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();
    try
    {
        // Get User Id
        int userId = await GetUserId(username);
        if (userId <= 0)
        {
            throw new ArgumentException($"Invalid user ID for username: {username}");
        }

        // Insert into Ball table and get the generated ID
        string insertBallQuery = @"
            INSERT INTO [Ball] (name, weight, hardness, core_type) 
            OUTPUT INSERTED.ballid 
            VALUES (@name, @weight, @hardness, @coretype)";

        using var ballCommand = new SqlCommand(insertBallQuery, connection, transaction);
        ballCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = ball.Name });
        ballCommand.Parameters.Add(new SqlParameter("@weight", SqlDbType.Float) { Value = ball.Weight });
        ballCommand.Parameters.Add(new SqlParameter("@hardness", SqlDbType.Float) { Value = ball.Hardness });
        ballCommand.Parameters.Add(new SqlParameter("@coretype", SqlDbType.NVarChar) { Value = ball.CoreType });

        object? result = await ballCommand.ExecuteScalarAsync();
        if (result == null || result == DBNull.Value)
        {
            throw new InvalidOperationException("Failed to insert ball or retrieve ball ID.");
        }

        int ballId = Convert.ToInt32(result);

        // Insert into Arsenal table
        string insertArsenalQuery = @"
            INSERT INTO [Arsenal] (userid, ball_id) 
            VALUES (@userid, @ballid)";

        using var arsenalCommand = new SqlCommand(insertArsenalQuery, connection, transaction);
        arsenalCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int) { Value = userId });
        arsenalCommand.Parameters.Add(new SqlParameter("@ballid", SqlDbType.Int) { Value = ballId });

        int rowsAffected = await arsenalCommand.ExecuteNonQueryAsync();
        if (rowsAffected <= 0)
        {
            throw new InvalidOperationException("Failed to associate ball with user in Arsenal.");
        }

        // Commit the transaction
        await transaction.CommitAsync();
        return true;
    }
    catch (Exception ex)
    {
        // Log exception
        LogWriter.LogError($"Error occurred while adding a ball for user '{username}': {ex.Message}\n\n"+ ex);

        // Rollback the transaction in case of any failure
        if (transaction != null)
        {
            await transaction.RollbackAsync();
        }

        throw; // Re-throw the exception after rollback
    }
}

}