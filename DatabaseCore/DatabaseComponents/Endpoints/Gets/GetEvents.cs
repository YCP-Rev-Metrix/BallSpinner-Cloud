using Common.POCOs;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Event>> GetEvents(string? username)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        
        string selectQuery = @"
            SELECT b.id, b.name, b.type, b.location, b.average, b.stats, b.standings
            FROM [combinedDB].[Events] b
            JOIN [User] u ON b.userId = u.id
            WHERE u.username = @Username;";
        
        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@Username", username ?? string.Empty);
        
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        List<Event> events = new List<Event>();
        while (await reader.ReadAsync())
        {
           Event eventObj = new Event
           {
               Id = reader["id"] as int?,
               Name = reader["name"] as string,
               Type = reader["type"] as string,
               Location = reader["location"] as string,
               Average = reader["average"] as int?,
               Stats = reader["stats"] as int?,
               Standings = reader["standings"] as string
           };
              events.Add(eventObj);
        }

        return events;
    }
}