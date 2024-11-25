using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<UserIdentification> users)> GetUsers()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT firstname, lastname FROM [User]"; // Adjusted to select more fields

        using var command = new SqlCommand(selectQuery, connection);

        var users = new List<UserIdentification>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var user = new UserIdentification
            {
                Firstname = reader["firstname"].ToString(),
                Lastname = reader["lastname"].ToString(),
            };

            users.Add(user);
        }

        return (users.Any(), users);
    }
}