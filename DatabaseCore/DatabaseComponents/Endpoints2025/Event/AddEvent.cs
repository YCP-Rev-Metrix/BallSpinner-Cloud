using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs.MobileApp;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddEvent(Event eventObj, string username, int? mobileID = null)
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
            int userId = mobileID.HasValue && mobileID.Value > 0
                ? await GetUserId(username, mobileID)
                : await GetUserId(username);
            if (userId <= 0)
            {
                LogWriter.LogError($"Invalid user ID for username: {username}");
                return false;
            }

            string insertEventQuery = @"
                INSERT INTO [combinedDB].[Events]
                    (userId, longName, nickName, type, location, startDate, endDate, weekDay, startTime, numGamesPerSession, average, schedule, stats, standings, enabled, mobileId)
                OUTPUT INSERTED.id
                VALUES
                    (@userId, @longName, @nickName, @type, @location, @startDate, @endDate, @weekDay, @startTime, @numGamesPerSession, @average, @schedule, @stats, @standings, @enabled, @mobileId)";

            using var eventCommand = new SqlCommand(insertEventQuery, connection, transaction);
            eventCommand.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            eventCommand.Parameters.Add("@longName", SqlDbType.VarChar, 200).Value = eventObj.LongName ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@nickName", SqlDbType.VarChar, 100).Value = eventObj.NickName ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@type", SqlDbType.VarChar, 50).Value = eventObj.Type ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@location", SqlDbType.VarChar, 100).Value = eventObj.Location ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@startDate", SqlDbType.VarChar, 20).Value = eventObj.StartDate ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@endDate", SqlDbType.VarChar, 20).Value = eventObj.EndDate ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@weekDay", SqlDbType.VarChar, 20).Value = eventObj.WeekDay ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@startTime", SqlDbType.VarChar, 10).Value = eventObj.StartTime ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@numGamesPerSession", SqlDbType.Int).Value = eventObj.NumGamesPerSession;
            eventCommand.Parameters.Add("@average", SqlDbType.Int).Value = eventObj.Average.HasValue ? (object)eventObj.Average.Value : DBNull.Value;
            eventCommand.Parameters.Add("@schedule", SqlDbType.VarChar, 500).Value = eventObj.Schedule ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@stats", SqlDbType.Int).Value = eventObj.Stats.HasValue ? (object)eventObj.Stats.Value : DBNull.Value;
            eventCommand.Parameters.Add("@standings", SqlDbType.VarChar, 500).Value = eventObj.Standings ?? (object)DBNull.Value;
            eventCommand.Parameters.Add("@enabled", SqlDbType.Bit).Value = eventObj.Enabled;
            eventCommand.Parameters.Add("@mobileId", SqlDbType.Int).Value = eventObj.MobileID.HasValue ? (object)eventObj.MobileID.Value : DBNull.Value;

            object? result = await eventCommand.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert event or retrieve event ID.");
            }

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
