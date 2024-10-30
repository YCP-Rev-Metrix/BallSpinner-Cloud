using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<(bool success, byte[] salt, string roles, byte[] hashedPassword)> GetUserValidData(string? username)
    {
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT roles, password, salt FROM [User] WHERE username = @Username";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            // Retrieve the columns
            string db_roles = reader["roles"].ToString();
            byte[] db_hashedPassword = (byte[])reader["password"];
            byte[] db_salt = (byte[])reader["salt"];

            return (true, db_salt, db_roles, db_hashedPassword);
        }
        else
        {
            return (false, null, null, null);
        }
    }
}