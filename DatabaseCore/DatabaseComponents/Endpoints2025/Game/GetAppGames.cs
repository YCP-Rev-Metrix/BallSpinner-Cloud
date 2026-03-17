using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<Game> games)> GetAppGames()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        const string selectQuery = "SELECT ID, GameNumber, Lanes, Score, Win, StartingLane, SessionID, TeamResult, IndividualResult FROM combinedDB.[Games]";

        using var command = new SqlCommand(selectQuery, connection);

        var games = new List<Game>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var game = new Game
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                GameNumber = reader["GameNumber"]?.ToString(),
                Lanes = reader["Lanes"]?.ToString(),
                Score = reader["Score"] != DBNull.Value ? Convert.ToInt32(reader["Score"]) : null,
                Win = reader["Win"] != DBNull.Value ? Convert.ToInt32(reader["Win"]) : null,
                StartingLane = reader["StartingLane"] != DBNull.Value ? Convert.ToInt32(reader["StartingLane"]) : null,
                SessionId = reader["SessionID"] != DBNull.Value ? Convert.ToInt32(reader["SessionID"]) : null,
                TeamResult = reader["TeamResult"] != DBNull.Value ? Convert.ToInt32(reader["TeamResult"]) : null,
                IndividualResult = reader["IndividualResult"] != DBNull.Value ? Convert.ToInt32(reader["IndividualResult"]) : null
            };

            games.Add(game);
        }

        return (games.Any(), games);
    }
}