using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<Session> users)> GetAppSessions()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID, SessionNumber, EstablishmentID, EventID, DateTime, TeamOpponent, IndividualOpponent, Score, Stats, TeamRecord, IndividualRecord FROM combinedDB.[Sessions]";
          
        using var command = new SqlCommand(selectQuery, connection);

        var sessions = new List<Session>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var session = new Session
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
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
            };

            sessions.Add(session);
        }

        return (sessions.Any(), sessions);
    }
}