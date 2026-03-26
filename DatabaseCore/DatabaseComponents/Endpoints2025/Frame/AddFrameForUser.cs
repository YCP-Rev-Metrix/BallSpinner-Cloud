using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddFrameForUser(Frame frame, string? username, int? mobileID = null)
    {
        if (frame == null) return false;
        if (!frame.GameId.HasValue || frame.GameId.Value <= 0) return false;

        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return false;

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Ensure the referenced Game belongs to the authenticated user (via Events ownership)
        const string ownsGameQuery = @"
            SELECT COUNT(1)
            FROM [combinedDB].[Games] g
            INNER JOIN [combinedDB].[Sessions] s ON s.ID = g.SessionID
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE g.ID = @GameId AND e.UserId = @UserId;";
        using (var ownsGameCmd = new SqlCommand(ownsGameQuery, connection))
        {
            ownsGameCmd.Parameters.Add("@GameId", SqlDbType.Int).Value = frame.GameId.Value;
            ownsGameCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            int owns = Convert.ToInt32(await ownsGameCmd.ExecuteScalarAsync());
            if (owns <= 0) return false;
        }

        const string insertQuery = @"
            INSERT INTO [combinedDB].[Frames]
                (gameId, shotOne, shotTwo, frameNumber, lane, result, mobileId)
            VALUES
                (@gameId, @shotOne, @shotTwo, @frameNumber, @lane, @result, @mobileId);";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@gameId", frame.GameId.Value);
        command.Parameters.AddWithValue("@mobileId", frame.MobileID.HasValue ? (object)frame.MobileID.Value : DBNull.Value);
        command.Parameters.AddWithValue("@shotOne", frame.ShotOne ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@shotTwo", frame.ShotTwo ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@frameNumber", frame.FrameNumber ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@lane", frame.Lane ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@result", frame.Result ?? (object)DBNull.Value);

        return await command.ExecuteNonQueryAsync() > 0;
    }
}

