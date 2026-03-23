using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, byte[]? hashedPassword)> GetCombinedUserValidData(string? username)
    {
        string? connectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        const string selectQuery = "SELECT HashedPassword FROM [combinedDB].[Users] WHERE Username = @Username";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = (object?)username ?? DBNull.Value;

        object? result = await command.ExecuteScalarAsync();
        if (result == null || result == DBNull.Value) return (false, null);
        return (true, (byte[])result);
    }
}

