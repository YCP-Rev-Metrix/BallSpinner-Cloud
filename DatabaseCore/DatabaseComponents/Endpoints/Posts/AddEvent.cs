using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using System.CodeDom.Compiler;
using Common.POCOs.MobileApp;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddEvent(Event eventObj, string username)
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

            string insertEventQuery = @"
                INSERT INTO [combinedDB].[Events] (userId, name, type, location, average, stats, standings)
                OUTPUT INSERTED.id
                VALUES (@userId, @name, @type, @location, @average, @stats, @standings)";
            
            using var eventCommand = new SqlCommand(insertEventQuery, connection, transaction);
            eventCommand.Parameters.AddWithValue("@userId", userId);
            eventCommand.Parameters.AddWithValue("@name", eventObj.Name ?? string.Empty);
            eventCommand.Parameters.AddWithValue("@type", eventObj.Type ?? string.Empty);
            eventCommand.Parameters.AddWithValue("@location", eventObj.Location ?? string.Empty);
            eventCommand.Parameters.AddWithValue("@average", eventObj.Average ?? 0);
            eventCommand.Parameters.AddWithValue("@stats", eventObj.Stats ?? 0);
            eventCommand.Parameters.AddWithValue("@standings", eventObj.Standings ?? string.Empty);
            
            object? result = await eventCommand.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert event or retrieve event ID.");
            }
            
            // Commit transaction
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            LogWriter.LogError($"Error occurred while adding an event for user '{username}': {ex.Message}\n{ex}");
            await transaction.RollbackAsync();
            throw;
        }
    }
}
