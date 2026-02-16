using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;


public partial class RevMetrixDb
{
    public async Task<bool> AddFrames(Frame frame, string? username)
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

            string insertFrameQuery = @"
            INSERT INTO [combinedDB].[Frames] (gameId, shotOne, shotTwo, frameNumber, lane, result)
            OUTPUT INSERTED.id
            VALUES (@gameId, @shotOne, @shotTwo, @frameNumber, @lane, @result)";

            using var frameCommand = new SqlCommand(insertFrameQuery, connection, transaction);
            frameCommand.Parameters.AddWithValue("@gameId", frame.GameId ?? (object)DBNull.Value);
            frameCommand.Parameters.AddWithValue("@shotOne", frame.ShotOne ?? (object)DBNull.Value);
            frameCommand.Parameters.AddWithValue("@shotTwo", frame.ShotTwo ?? (object)DBNull.Value);
            frameCommand.Parameters.AddWithValue("@frameNumber", frame.FrameNumber ?? (object)DBNull.Value);
            frameCommand.Parameters.AddWithValue("@lane", frame.Lane ?? (object)DBNull.Value);
            frameCommand.Parameters.AddWithValue("@result", frame.Result ?? (object)DBNull.Value);
            
            object? result = await frameCommand.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert frame or retrieve frame ID.");
            }
            
            // Commit transaction
            await transaction.CommitAsync();
            return true;
        } catch (Exception ex)
        {
            LogWriter.LogError($"Error occurred while adding a frame for user '{username}': {ex.Message}\n{ex}");
            await transaction.RollbackAsync();
            throw;
        }
    }
}