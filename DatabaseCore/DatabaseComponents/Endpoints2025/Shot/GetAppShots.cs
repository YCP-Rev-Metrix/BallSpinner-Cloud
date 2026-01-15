using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<ShotTable> shots)> GetAppShots()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID FROM combinedDB.[Shots]"; // Adjusted to select more fields

        using var command = new SqlCommand(selectQuery, connection);

        var shots = new List<ShotTable>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var shot = new ShotTable
            {
                ID = (int)reader["ID"]
            };

            shots.Add(shot);
        }

        return (shots.Any(), shots);
    }
}