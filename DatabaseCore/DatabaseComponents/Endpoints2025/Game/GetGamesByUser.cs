using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Game>> GetGamesByUser(string? username, int? mobileID = null)
    {
        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return new List<Game>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        const string selectQuery = @"
            SELECT g.ID, g.GameNumber, g.Lanes, g.Score, g.Win, g.StartingLane, g.SessionID, g.TeamResult, g.IndividualResult, g.MobileID
            FROM [combinedDB].[Games] g
            INNER JOIN [combinedDB].[Sessions] s ON s.ID = g.SessionID
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE e.UserId = @UserId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        var games = new List<Game>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            games.Add(new Game
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                MobileID = reader["MobileID"] != DBNull.Value ? Convert.ToInt32(reader["MobileID"]) : null,
                GameNumber = reader["GameNumber"]?.ToString(),
                Lanes = reader["Lanes"]?.ToString(),
                Score = reader["Score"] != DBNull.Value ? Convert.ToInt32(reader["Score"]) : null,
                Win = reader["Win"] != DBNull.Value ? Convert.ToInt32(reader["Win"]) : null,
                StartingLane = reader["StartingLane"] != DBNull.Value ? Convert.ToInt32(reader["StartingLane"]) : null,
                SessionId = reader["SessionID"] != DBNull.Value ? Convert.ToInt32(reader["SessionID"]) : null,
                TeamResult = reader["TeamResult"] != DBNull.Value ? Convert.ToInt32(reader["TeamResult"]) : null,
                IndividualResult = reader["IndividualResult"] != DBNull.Value ? Convert.ToInt32(reader["IndividualResult"]) : null
            });
        }

        return games;
    }
}

