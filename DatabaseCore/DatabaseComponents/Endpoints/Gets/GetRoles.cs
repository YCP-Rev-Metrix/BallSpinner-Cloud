using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<(bool success, string roles)> GetRoles(string username)
    {
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT roles FROM [User] WHERE username = @Username";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            // Retrieve the "roles" value
            string roles = reader["roles"].ToString();

            return (true, roles);
        }
        else
        {
            return (false, null);
        }
    }
}