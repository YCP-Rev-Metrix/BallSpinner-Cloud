using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<UserTable> users)> GetAppUsers()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID, Firstname, Lastname, Username, HashedPassword, Email, PhoneNumber, LastLogin, Hand FROM combinedDB.[Users]"; // Adjusted to select more fields
        
        using var command = new SqlCommand(selectQuery, connection);

        var users = new List<UserTable>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var user = new UserTable
            {
                Id = (int)reader["ID"],
                Firstname = reader["Firstname"] as string,
                Lastname = reader["Lastname"] as string,
                Username = reader["Username"] as string,
                HashedPassword = reader["HashedPassword"] as byte[],
                Email = reader["Email"] as string,
                PhoneNumber = reader["PhoneNumber"] as string,
                LastLogin = reader["LastLogin"] as string,
                Hand = reader["Hand"] as string

            };

            users.Add(user);
        }

        return (users.Any(), users);
    }
}