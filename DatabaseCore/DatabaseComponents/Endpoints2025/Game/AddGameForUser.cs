using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddGameForUser(Game game, string? username, int? mobileID = null)
    {
        if (game == null) return false;
        if (!game.SessionId.HasValue || game.SessionId.Value <= 0) return false;

        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return false;

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Ensure the referenced Session belongs to the authenticated user (via Events ownership)
        const string ownsSessionQuery = @"
            SELECT COUNT(1)
            FROM [combinedDB].[Sessions] s
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE s.ID = @SessionId AND e.UserId = @UserId;";
        using (var ownsSessionCmd = new SqlCommand(ownsSessionQuery, connection))
        {
            ownsSessionCmd.Parameters.Add("@SessionId", SqlDbType.Int).Value = game.SessionId.Value;
            ownsSessionCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            int owns = Convert.ToInt32(await ownsSessionCmd.ExecuteScalarAsync());
            if (owns <= 0) return false;
        }

        const string insertQuery = @"
            INSERT INTO [combinedDB].[Games]
                (GameNumber, Lanes, Score, Win, StartingLane, SessionID, TeamResult, IndividualResult, MobileID)
            VALUES
                (@GameNumber, @Lanes, @Score, @Win, @StartingLane, @SessionID, @TeamResult, @IndividualResult, @MobileID);";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@GameNumber", SqlDbType.VarChar).Value = game.GameNumber ?? string.Empty;
        command.Parameters.Add("@Lanes", SqlDbType.VarChar).Value = game.Lanes ?? string.Empty;
        command.Parameters.Add("@Score", SqlDbType.Int).Value = game.Score ?? (object)DBNull.Value;
        command.Parameters.Add("@Win", SqlDbType.Int).Value = game.Win ?? (object)DBNull.Value;
        command.Parameters.Add("@StartingLane", SqlDbType.Int).Value = game.StartingLane ?? (object)DBNull.Value;
        command.Parameters.Add("@SessionID", SqlDbType.Int).Value = game.SessionId.Value;
        command.Parameters.Add("@TeamResult", SqlDbType.Int).Value = game.TeamResult ?? (object)DBNull.Value;
        command.Parameters.Add("@IndividualResult", SqlDbType.Int).Value = game.IndividualResult ?? (object)DBNull.Value;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = game.MobileID.HasValue ? (object)game.MobileID.Value : DBNull.Value;

        return await command.ExecuteNonQueryAsync() > 0;
    }
}

