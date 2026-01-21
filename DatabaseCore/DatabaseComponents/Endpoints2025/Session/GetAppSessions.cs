using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<SessionTable> users)> GetAppSessions()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID, SessionNumber, EstablishmentID, EventID, DateTime, TeamOpponent, IndividualOpponent, Score, Stats, TeamRecord, IndividualRecord FROM combinedDB.[Sessions]"; // Adjusted to select more fields
          
        using var command = new SqlCommand(selectQuery, connection);

        var sessions = new List<SessionTable>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var session = new SessionTable
            {
                ID = (int)reader["ID"],
                SessionNumber = (int)reader["SessionNumber"],
                EstablishmentID = (int)reader["EstablishmentID"],
                EventID = (int)reader["EventID"],
                DateTime = (int)reader["DateTime"],
                TeamOpponent = reader["TeamOpponent"] as string,
                IndividualOpponent = reader["IndividualOpponent"] as string,
                Score = (int)reader["Score"],
                Stats = (int)reader["Stats"],
                TeamRecord = (int)reader["TeamRecord"],
                IndividualRecord = (int)reader["IndividualRecord"]

            };

            sessions.Add(session);
        }

        return (sessions.Any(), sessions);
    }
}