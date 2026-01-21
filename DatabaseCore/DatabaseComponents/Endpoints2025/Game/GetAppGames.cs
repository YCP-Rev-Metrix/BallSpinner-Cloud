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

        const string selectQuery = "SELECT ID, GameNumber, Lanes, Score, Win, StartingLane, SessionID, TeamResult, IndividualResult FROM combinedDB.[Games]"; // Adjusted to select more fields

        using var command = new SqlCommand(selectQuery, connection);

        var games = new List<GameTable>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var game = new GameTable
            {
                ID = (int)reader["ID"],
                GameNumber = reader["GameNumber"].ToString() ?? string.Empty,
                Lanes = reader["Lanes"].ToString() ?? string.Empty,
                Score = (int)reader["Score"],
                Win = (int)reader["Win"],
                StartingLane = (int)reader["StartingLane"],
                SessionID = (int)reader["SessionID"],
                TeamResult = (int)reader["TeamResult"],
                IndividualResult = (int)reader["IndividualResult"]
            };

            games.Add(game);
        }

        return (games.Any(), games);
    }
}