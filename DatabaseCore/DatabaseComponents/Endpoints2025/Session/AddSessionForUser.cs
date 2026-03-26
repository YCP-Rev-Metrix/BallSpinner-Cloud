using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddSessionForUser(Session session, string? username, int? mobileID = null)
    {
        if (session == null) return false;
        if (!session.EventId.HasValue || session.EventId.Value <= 0) return false;

        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return false;

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Ensure the referenced Event belongs to the authenticated user
        const string ownsEventQuery = @"SELECT COUNT(1) FROM [combinedDB].[Events] WHERE ID = @EventId AND UserId = @UserId;";
        using (var ownsEventCmd = new SqlCommand(ownsEventQuery, connection))
        {
            ownsEventCmd.Parameters.Add("@EventId", SqlDbType.Int).Value = session.EventId.Value;
            ownsEventCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            int owns = Convert.ToInt32(await ownsEventCmd.ExecuteScalarAsync());
            if (owns <= 0) return false;
        }

        const string insertQuery = @"
            INSERT INTO [combinedDB].[Sessions]
                (SessionNumber, EstablishmentID, EventID, DateTime, TeamOpponent, IndividualOpponent, Score, Stats, TeamRecord, IndividualRecord, MobileID)
            VALUES
                (@SessionNumber, @EstablishmentID, @EventID, @DateTime, @TeamOpponent, @IndividualOpponent, @Score, @Stats, @TeamRecord, @IndividualRecord, @MobileID);";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@SessionNumber", SqlDbType.Int).Value = session.SessionNumber ?? (object)DBNull.Value;
        command.Parameters.Add("@EstablishmentID", SqlDbType.Int).Value = session.EstablishmentId ?? (object)DBNull.Value;
        command.Parameters.Add("@EventID", SqlDbType.Int).Value = session.EventId.Value;
        command.Parameters.Add("@DateTime", SqlDbType.Int).Value = session.DateTime ?? (object)DBNull.Value;
        command.Parameters.Add("@TeamOpponent", SqlDbType.VarChar).Value = session.TeamOpponent ?? string.Empty;
        command.Parameters.Add("@IndividualOpponent", SqlDbType.VarChar).Value = session.IndividualOpponent ?? string.Empty;
        command.Parameters.Add("@Score", SqlDbType.Int).Value = session.Score ?? (object)DBNull.Value;
        command.Parameters.Add("@Stats", SqlDbType.Int).Value = session.Stats ?? (object)DBNull.Value;
        command.Parameters.Add("@TeamRecord", SqlDbType.Int).Value = session.TeamRecord ?? (object)DBNull.Value;
        command.Parameters.Add("@IndividualRecord", SqlDbType.Int).Value = session.IndividualRecord ?? (object)DBNull.Value;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = session.MobileID.HasValue ? (object)session.MobileID.Value : DBNull.Value;

        return await command.ExecuteNonQueryAsync() > 0;
    }
}

