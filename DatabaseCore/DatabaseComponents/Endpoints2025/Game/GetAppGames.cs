using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<GameTable> games)> GetAppGames()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID FROM combinedDB.[Games]"; // Adjusted to select more fields

        using var command = new SqlCommand(selectQuery, connection);

        var games = new List<GameTable>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var game = new GameTable
            {
                ID = (int)reader["ID"]
            };

            games.Add(game);
        }

        return (games.Any(), games);
    }
}