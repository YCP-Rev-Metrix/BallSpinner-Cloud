using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Ball>> GetBalls(string? username, int? mobileID = null)
    {
        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return new List<Ball>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Inserts use combinedDB user id from GetUserId; legacy JOIN [User] hid phone-app rows.
        string selectQuery = @"
            SELECT b.id, b.userId, b.name, b.weight, b.coreType, b.mobileId
            FROM [combinedDB].[Balls] b
            WHERE b.userId = @UserId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        List<Ball> balls = new List<Ball>();
        while (await reader.ReadAsync())
        {
            var fetchedBall = new Ball
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                MobileID = reader["mobileId"] != DBNull.Value && reader["mobileId"] != null ? Convert.ToInt32(reader["mobileId"]) : null,
                UserId = reader["userId"] != DBNull.Value ? Convert.ToInt32(reader["userId"]) : null,
                Name = reader["name"] as string,
                Weight = reader["weight"]?.ToString(),
                CoreType = reader["coreType"] as string
            };
            balls.Add(fetchedBall);
        }

        return balls;
    }
}
