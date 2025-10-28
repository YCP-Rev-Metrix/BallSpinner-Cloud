using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<SessionTable> users)> GetAppSessions()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID FROM combinedDB.[Sessions]"; // Adjusted to select more fields

        using var command = new SqlCommand(selectQuery, connection);

        var sessions = new List<SessionTable>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var session = new SessionTable
            {
                ID = (int)reader["ID"]
            };

            sessions.Add(session);
        }

        return (sessions.Any(), sessions);
    }
}