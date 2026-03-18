using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Frame>> GetFrames(int gameId)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = @"
            SELECT id, gameId, shotOne, shotTwo, frameNumber, lane, result
            FROM [combinedDB].[Frames]
            WHERE gameId = @gameId
        ";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@gameId", gameId);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        List<Frame> frames = new List<Frame>();
        while (await reader.ReadAsync())
        {
            var frame = new Frame
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                GameId = reader["gameId"] != DBNull.Value ? Convert.ToInt32(reader["gameId"]) : null,
                ShotOne = reader["shotOne"] != DBNull.Value ? Convert.ToInt32(reader["shotOne"]) : null,
                ShotTwo = reader["shotTwo"] != DBNull.Value ? Convert.ToInt32(reader["shotTwo"]) : null,
                FrameNumber = reader["frameNumber"] != DBNull.Value ? Convert.ToInt32(reader["frameNumber"]) : null,
                Lane = reader["lane"] != DBNull.Value ? Convert.ToInt32(reader["lane"]) : null,
                Result = reader["result"] != DBNull.Value ? Convert.ToInt32(reader["result"]) : null
            };
            frames.Add(frame);
        }
        return frames;
    }
}
