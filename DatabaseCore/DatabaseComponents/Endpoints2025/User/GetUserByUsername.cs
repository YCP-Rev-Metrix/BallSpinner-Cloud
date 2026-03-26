using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<User>> GetAppUserByUsername(string? username, int? mobileID = null)
    {
        if (string.IsNullOrWhiteSpace(username)) return new List<User>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = mobileID.HasValue && mobileID.Value > 0
            ? @"SELECT ID, Firstname, Lastname, Username, HashedPassword, Email, PhoneNumber, LastLogin, Hand, MobileID
                FROM [combinedDB].[Users]
                WHERE Username = @Username AND MobileID = @MobileID;"
            : @"SELECT ID, Firstname, Lastname, Username, HashedPassword, Email, PhoneNumber, LastLogin, Hand, MobileID
                FROM [combinedDB].[Users]
                WHERE Username = @Username;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
        if (mobileID.HasValue && mobileID.Value > 0)
            command.Parameters.Add("@MobileID", SqlDbType.Int).Value = mobileID.Value;

        var users = new List<User>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                MobileID = reader["MobileID"] != DBNull.Value ? Convert.ToInt32(reader["MobileID"]) : null,
                Firstname = reader["Firstname"] as string,
                Lastname = reader["Lastname"] as string,
                Username = reader["Username"] as string,
                HashedPassword = reader["HashedPassword"] as byte[],
                Email = reader["Email"] as string,
                PhoneNumber = reader["PhoneNumber"] as string,
                LastLogin = reader["LastLogin"] as string,
                Hand = reader["Hand"] as string
            });
        }

        return users;
    }
}

