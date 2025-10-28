using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<UserTable> users)> GetAppUsers()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID FROM combinedDB.[Users]"; // Adjusted to select more fields

        using var command = new SqlCommand(selectQuery, connection);

        var users = new List<UserTable>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var user = new UserTable
            {
                Id = (int)reader["ID"]
            };

            users.Add(user);
        }

        return (users.Any(), users);
    }
}