using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Establishment>> GetEstablishmentsByUser(string? username, int? mobileID = null)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Establishments are shared venue data (bowling alleys), not user-owned records.
        // Return all so the client can always resolve a posted establishment's cloud ID by mobileID,
        // even before any session has linked to it.
        const string selectQuery = @"
            SELECT ID, Name, Lanes, Type, Location, MobileID
            FROM [combinedDB].[Establishments];";

        using var command = new SqlCommand(selectQuery, connection);

        var establishments = new List<Establishment>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            establishments.Add(new Establishment
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                MobileID = reader["MobileID"] != DBNull.Value ? Convert.ToInt32(reader["MobileID"]) : null,
                Name = reader["Name"] as string,
                Lanes = reader["Lanes"] as string,
                Type = reader["Type"] as string,
                Location = reader["Location"] as string
            });
        }

        return establishments;
    }
}

