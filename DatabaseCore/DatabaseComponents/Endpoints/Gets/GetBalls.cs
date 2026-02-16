using Common.POCOs;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Ball>> GetBalls(string? username)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        
        string selectQuery = @"
            SELECT b.name, b.weight, b.coreType
            FROM [combinedDB].[Balls] b
            JOIN [User] u ON b.userId = u.id
            WHERE u.username = @Username;";
        
        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@Username", username ?? string.Empty);
        
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        List<Ball> balls = new List<Ball>();
        while (await reader.ReadAsync())
        {
            var fetchedBall = new Ball
            {
                Name = reader["name"] as string,
                Weight = reader["weight"] != DBNull.Value ? Convert.ToInt32(reader["weight"]) : 0,
                CoreType = reader["coreType"] as string
            };
            balls.Add(fetchedBall);
        }

        return balls;
    }
}