using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Session>> GetSessionsByUser(string? username, int? mobileID = null)
    {
        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return new List<Session>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        const string selectQuery = @"
            SELECT s.ID, s.SessionNumber, s.EstablishmentID, s.EventID, s.DateTime, s.TeamOpponent, s.IndividualOpponent,
                   s.Score, s.Stats, s.TeamRecord, s.IndividualRecord, s.MobileID
            FROM [combinedDB].[Sessions] s
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE e.UserId = @UserId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        var sessions = new List<Session>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            sessions.Add(new Session
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                MobileID = reader["MobileID"] != DBNull.Value ? Convert.ToInt32(reader["MobileID"]) : null,
                SessionNumber = reader["SessionNumber"] != DBNull.Value ? Convert.ToInt32(reader["SessionNumber"]) : null,
                EstablishmentId = reader["EstablishmentID"] != DBNull.Value ? Convert.ToInt32(reader["EstablishmentID"]) : null,
                EventId = reader["EventID"] != DBNull.Value ? Convert.ToInt32(reader["EventID"]) : null,
                DateTime = reader["DateTime"] != DBNull.Value ? Convert.ToInt32(reader["DateTime"]) : null,
                TeamOpponent = reader["TeamOpponent"] as string,
                IndividualOpponent = reader["IndividualOpponent"] as string,
                Score = reader["Score"] != DBNull.Value ? Convert.ToInt32(reader["Score"]) : null,
                Stats = reader["Stats"] != DBNull.Value ? Convert.ToInt32(reader["Stats"]) : null,
                TeamRecord = reader["TeamRecord"] != DBNull.Value ? Convert.ToInt32(reader["TeamRecord"]) : null,
                IndividualRecord = reader["IndividualRecord"] != DBNull.Value ? Convert.ToInt32(reader["IndividualRecord"]) : null
            });
        }

        return sessions;
    }
}

