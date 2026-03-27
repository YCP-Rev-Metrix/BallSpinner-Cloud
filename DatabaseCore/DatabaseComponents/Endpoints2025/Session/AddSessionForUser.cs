using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddSessionForUser(Session session, string? username, int? mobileID = null)
    {
        if (session == null) return false;
        if (!session.EventId.HasValue || session.EventId.Value <= 0)
        {
            LogWriter.LogWarn(
                $"AddSessionForUser: invalid or missing eventId (username={username}, mobileID={mobileID}).");
            return false;
        }

        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0)
        {
            LogWriter.LogWarn(
                $"AddSessionForUser: no user row for username={username}, mobileID={mobileID} (combinedDB.Users / [User]).");
            return false;
        }

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Clients often send device-local event id (matches Events.mobileId) rather than cloud Events.id.
        // Accept either: resolve to the canonical cloud id first by primary key, then by mobileId + user.
        int? cloudEventId = await ResolveOwnedEventCloudIdAsync(connection, session.EventId.Value, userId);
        if (!cloudEventId.HasValue)
        {
            cloudEventId = await TrySingleOwnedEventFallbackAsync(connection, userId);
            if (!cloudEventId.HasValue)
            {
                int eventCount = await CountEventsForUserAsync(connection, userId);
                LogWriter.LogWarn(
                    eventCount == 0
                        ? $"AddSessionForUser: no Events rows for user (userId={userId}, username={username}). " +
                          $"Post events before sessions, or send Events.id / Events.MobileID as eventId (client sent {session.EventId.Value})."
                        : $"AddSessionForUser: event not found or not owned by user (eventId from client={session.EventId.Value}, userId={userId}, username={username}). " +
                          $"User has {eventCount} event(s); expected cloud Events.Id or Events.MobileID to match one of them.");
                return false;
            }

            LogWriter.LogWarn(
                $"AddSessionForUser: eventId from client ({session.EventId.Value}) did not match any Event; " +
                $"using sole event id {cloudEventId.Value} for userId={userId}. Prefer sending cloud id or Events.MobileID.");
        }

        const string insertQuery = @"
            INSERT INTO [combinedDB].[Sessions]
                (SessionNumber, EstablishmentID, EventID, DateTime, TeamOpponent, IndividualOpponent, Score, Stats, TeamRecord, IndividualRecord, MobileID)
            VALUES
                (@SessionNumber, @EstablishmentID, @EventID, @DateTime, @TeamOpponent, @IndividualOpponent, @Score, @Stats, @TeamRecord, @IndividualRecord, @MobileID);";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@SessionNumber", SqlDbType.Int).Value = session.SessionNumber ?? (object)DBNull.Value;
        command.Parameters.Add("@EstablishmentID", SqlDbType.Int).Value = session.EstablishmentId ?? (object)DBNull.Value;
        command.Parameters.Add("@EventID", SqlDbType.Int).Value = cloudEventId.Value;
        command.Parameters.Add("@DateTime", SqlDbType.Int).Value = session.DateTime ?? (object)DBNull.Value;
        command.Parameters.Add("@TeamOpponent", SqlDbType.VarChar).Value = session.TeamOpponent ?? string.Empty;
        command.Parameters.Add("@IndividualOpponent", SqlDbType.VarChar).Value = session.IndividualOpponent ?? string.Empty;
        command.Parameters.Add("@Score", SqlDbType.Int).Value = session.Score ?? (object)DBNull.Value;
        command.Parameters.Add("@Stats", SqlDbType.Int).Value = session.Stats ?? (object)DBNull.Value;
        command.Parameters.Add("@TeamRecord", SqlDbType.Int).Value = session.TeamRecord ?? (object)DBNull.Value;
        command.Parameters.Add("@IndividualRecord", SqlDbType.Int).Value = session.IndividualRecord ?? (object)DBNull.Value;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = session.MobileID.HasValue ? (object)session.MobileID.Value : DBNull.Value;

        try
        {
            int rows = await command.ExecuteNonQueryAsync();
            if (rows <= 0)
            {
                LogWriter.LogWarn(
                    $"AddSessionForUser: INSERT affected 0 rows (cloudEventId={cloudEventId.Value}, userId={userId}).");
            }

            return rows > 0;
        }
        catch (SqlException ex)
        {
            LogWriter.LogError(
                $"AddSessionForUser: SQL error — {ex.Message} (cloudEventId={cloudEventId.Value}, userId={userId}, establishmentId={session.EstablishmentId}).");
            throw;
        }
    }

    /// <summary>
    /// Maps client-supplied event reference to cloud Events row <c>id</c> for this user.
    /// Tries primary key first, then <c>mobileId</c> (device-local id stored when the event was posted).
    /// </summary>
    private static async Task<int?> ResolveOwnedEventCloudIdAsync(SqlConnection connection, int eventIdFromClient, int userId)
    {
        const string byIdQuery =
            @"SELECT TOP 1 id FROM [combinedDB].[Events] WHERE id = @EventId AND userId = @UserId;";
        using (var cmd = new SqlCommand(byIdQuery, connection))
        {
            cmd.Parameters.Add("@EventId", SqlDbType.Int).Value = eventIdFromClient;
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            object? o = await cmd.ExecuteScalarAsync();
            if (o != null && o != DBNull.Value)
                return Convert.ToInt32(o);
        }

        const string byMobileQuery =
            @"SELECT TOP 1 id FROM [combinedDB].[Events] WHERE mobileId = @MobileEventId AND userId = @UserId ORDER BY id DESC;";
        using (var cmd = new SqlCommand(byMobileQuery, connection))
        {
            cmd.Parameters.Add("@MobileEventId", SqlDbType.Int).Value = eventIdFromClient;
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            object? o = await cmd.ExecuteScalarAsync();
            if (o != null && o != DBNull.Value)
                return Convert.ToInt32(o);
        }

        return null;
    }

    /// <summary>
    /// When the client sends a placeholder id (e.g. 1) and this user has exactly one event, use that row.
    /// </summary>
    private static async Task<int?> TrySingleOwnedEventFallbackAsync(SqlConnection connection, int userId)
    {
        const string query = @"
            SELECT id
            FROM [combinedDB].[Events]
            WHERE userId = @UserId;";
        var ids = new List<int>();
        using (var cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                ids.Add(reader.GetInt32(0));
        }

        return ids.Count == 1 ? ids[0] : null;
    }

    private static async Task<int> CountEventsForUserAsync(SqlConnection connection, int userId)
    {
        const string query = @"SELECT COUNT(1) FROM [combinedDB].[Events] WHERE userId = @UserId;";
        using var cmd = new SqlCommand(query, connection);
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
        object? o = await cmd.ExecuteScalarAsync();
        return o != null && o != DBNull.Value ? Convert.ToInt32(o) : 0;
    }
}

