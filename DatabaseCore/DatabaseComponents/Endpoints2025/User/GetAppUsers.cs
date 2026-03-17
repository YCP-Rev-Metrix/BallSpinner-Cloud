using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<User> users)> GetAppUsers()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID, Firstname, Lastname, Username, HashedPassword, Email, PhoneNumber, LastLogin, Hand FROM combinedDB.[Users]";
        
        using var command = new SqlCommand(selectQuery, connection);

        var users = new List<User>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var user = new User
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
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