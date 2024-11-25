using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, string username, DateTime? expiration)> RemoveRefreshToken(byte[] token)
    {
        // Remove row where token matches token param
        // Retrieve username and expiration at same time

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string insertQuery = "DELETE FROM [Token] " +
            "OUTPUT DELETED.expiration, [User].username " +
            "FROM [Token] " +
            "INNER JOIN [User] ON [Token].userid = [User].id " +
            "WHERE [Token].token = @Token;";

        using var command = new SqlCommand(insertQuery, connection);
        // Set the parameter values
        command.Parameters.Add("@Token", SqlDbType.VarBinary, 32).Value = token;

        // Execute the query
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            // Retrieve the columns
            string username = reader["username"].ToString();
            var expiration = (DateTime)reader["expiration"];

            return (true, username, expiration);
        }
        else
        {
            return (false, null, null);
        }
    }
}