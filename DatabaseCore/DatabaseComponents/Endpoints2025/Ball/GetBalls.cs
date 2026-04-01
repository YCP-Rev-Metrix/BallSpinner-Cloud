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

        string selectQuery = @"
            SELECT b.id, b.userId, b.name, b.ballMFG, b.ballMFGName, b.serialNumber, b.weight,
                   b.core, b.colorString, b.coverstock, b.comment, b.enabled, b.mobileId
            FROM [combinedDB].[Balls] b
            WHERE b.userId = @UserId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        List<Ball> balls = new List<Ball>();
        while (await reader.ReadAsync())
        {
            balls.Add(new Ball
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                MobileID = reader["mobileId"] != DBNull.Value ? Convert.ToInt32(reader["mobileId"]) : null,
                UserId = reader["userId"] != DBNull.Value ? Convert.ToInt32(reader["userId"]) : null,
                Name = reader["name"] as string,
                BallMFG = reader["ballMFG"] as string,
                BallMFGName = reader["ballMFGName"] as string,
                SerialNumber = reader["serialNumber"] as string,
                Weight = reader["weight"] != DBNull.Value ? Convert.ToInt32(reader["weight"]) : null,
                Core = reader["core"] as string,
                ColorString = reader["colorString"] as string,
                Coverstock = reader["coverstock"] as string,
                Comment = reader["comment"] as string,
                Enabled = reader["enabled"] != DBNull.Value && Convert.ToBoolean(reader["enabled"])
            });
        }

        return balls;
    }
}
