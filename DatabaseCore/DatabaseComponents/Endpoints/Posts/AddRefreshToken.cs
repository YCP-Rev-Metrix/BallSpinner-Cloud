using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddRefreshToken(byte[] token, string? username, DateTime expiration)
    {
        // Look into user table, get id that matches with username
        // add row to token table (token, id, expiration)
        string ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string insertQuery = "INSERT INTO [RefreshToken] (token, userid, expiration) " +
            "VALUES (@Token, (SELECT id FROM [User] WHERE username = @Username), @Expiration);";

        using var command = new SqlCommand(insertQuery, connection);
        // Set the parameter values
        command.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
        command.Parameters.Add("@Token", SqlDbType.VarBinary, 32).Value = token;
        command.Parameters.Add("@Expiration", SqlDbType.DateTime).Value = expiration;

        // Execute the query
        int i = await command.ExecuteNonQueryAsync();
        return i != -1;
    }
}