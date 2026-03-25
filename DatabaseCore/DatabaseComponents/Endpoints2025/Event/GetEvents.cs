using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Event>> GetEvents(string? username, int? mobileID = null)
    {
        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return new List<Event>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = @"
            SELECT b.id, b.userId, b.name, b.type, b.location, b.average, b.stats, b.standings, b.mobileId
            FROM [combinedDB].[Events] b
            WHERE b.userId = @UserId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        List<Event> events = new List<Event>();
        while (await reader.ReadAsync())
        {
            var eventObj = new Event
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                UserId = reader["userId"] != DBNull.Value ? Convert.ToInt32(reader["userId"]) : 0,
                MobileID = reader["mobileId"] != DBNull.Value && reader["mobileId"] != null ? Convert.ToInt32(reader["mobileId"]) : null,
                Name = reader["name"] as string,
                Type = reader["type"] as string,
                Location = reader["location"] as string,
                Average = reader["average"] != DBNull.Value ? Convert.ToInt32(reader["average"]) : null,
                Stats = reader["stats"] != DBNull.Value ? Convert.ToInt32(reader["stats"]) : null,
                Standings = reader["standings"] as string
            };
            events.Add(eventObj);
        }

        return events;
    }
}
