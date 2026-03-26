using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Frame>> GetFramesByUser(string? username, int? mobileID = null)
    {
        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return new List<Frame>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        const string selectQuery = @"
            SELECT f.id, f.gameId, f.shotOne, f.shotTwo, f.frameNumber, f.lane, f.result, f.mobileId
            FROM [combinedDB].[Frames] f
            INNER JOIN [combinedDB].[Games] g ON g.ID = f.gameId
            INNER JOIN [combinedDB].[Sessions] s ON s.ID = g.SessionID
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE e.UserId = @UserId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        List<Frame> frames = new();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            frames.Add(new Frame
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                MobileID = reader["mobileId"] != DBNull.Value ? Convert.ToInt32(reader["mobileId"]) : null,
                GameId = reader["gameId"] != DBNull.Value ? Convert.ToInt32(reader["gameId"]) : null,
                ShotOne = reader["shotOne"] != DBNull.Value ? Convert.ToInt32(reader["shotOne"]) : null,
                ShotTwo = reader["shotTwo"] != DBNull.Value ? Convert.ToInt32(reader["shotTwo"]) : null,
                FrameNumber = reader["frameNumber"] != DBNull.Value ? Convert.ToInt32(reader["frameNumber"]) : null,
                Lane = reader["lane"] != DBNull.Value ? Convert.ToInt32(reader["lane"]) : null,
                Result = reader["result"] != DBNull.Value ? Convert.ToInt32(reader["result"]) : null
            });
        }

        return frames;
    }

    public async Task<List<Frame>> GetFramesByGameIdForUser(string? username, int? mobileID, int gameId)
    {
        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return new List<Frame>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        const string selectQuery = @"
            SELECT f.id, f.gameId, f.shotOne, f.shotTwo, f.frameNumber, f.lane, f.result, f.mobileId
            FROM [combinedDB].[Frames] f
            INNER JOIN [combinedDB].[Games] g ON g.ID = f.gameId
            INNER JOIN [combinedDB].[Sessions] s ON s.ID = g.SessionID
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE e.UserId = @UserId AND f.gameId = @GameId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
        command.Parameters.Add("@GameId", SqlDbType.Int).Value = gameId;

        List<Frame> frames = new();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            frames.Add(new Frame
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                MobileID = reader["mobileId"] != DBNull.Value ? Convert.ToInt32(reader["mobileId"]) : null,
                GameId = reader["gameId"] != DBNull.Value ? Convert.ToInt32(reader["gameId"]) : null,
                ShotOne = reader["shotOne"] != DBNull.Value ? Convert.ToInt32(reader["shotOne"]) : null,
                ShotTwo = reader["shotTwo"] != DBNull.Value ? Convert.ToInt32(reader["shotTwo"]) : null,
                FrameNumber = reader["frameNumber"] != DBNull.Value ? Convert.ToInt32(reader["frameNumber"]) : null,
                Lane = reader["lane"] != DBNull.Value ? Convert.ToInt32(reader["lane"]) : null,
                Result = reader["result"] != DBNull.Value ? Convert.ToInt32(reader["result"]) : null
            });
        }

        return frames;
    }
}

