using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;
using System.CodeDom.Compiler;
namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddBalls(Ball ball, string username)
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

            // Insert Ball and retrieve Ball ID
            string insertBallQuery = @"
            INSERT INTO [combinedDB].[Balls] (userId, name, weight, coreType) 
            OUTPUT INSERTED.id 
            VALUES (@userId, @name, @weight, @coretype)";

            using var ballCommand = new SqlCommand(insertBallQuery, connection, transaction);
            ballCommand.Parameters.AddWithValue("@name", ball.Name ?? string.Empty);
            ballCommand.Parameters.AddWithValue("@userId", userId);
            ballCommand.Parameters.AddWithValue("@weight", ball.Weight);
            ballCommand.Parameters.AddWithValue("@coretype", ball.CoreType ?? string.Empty);

            object? result = await ballCommand.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert ball or retrieve ball ID.");
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