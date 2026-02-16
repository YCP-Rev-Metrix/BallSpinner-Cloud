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
            Frame frame = new Frame
            {
                Id = reader["id"] as int?,
                GameId = reader["gameId"] as int?,
                ShotOne = reader["shotOne"] as int?,
                ShotTwo = reader["shotTwo"] as int?,
                FrameNumber = reader["frameNumber"] as int?,
                Lane = reader["lane"] as int?,
                Result = reader["result"] as int?
            };
            frames.Add(frame);
        }
        return frames;
    }
}