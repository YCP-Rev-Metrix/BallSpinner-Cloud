using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddRefreshToken(byte[] token, string? username, DateTime expiration)
    {
        string ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // RefreshToken table references legacy [User]. Combined-only app users may not exist there.
        // In that case, allow auth to succeed without storing a refresh token row.
        const string selectUserIdQuery = "SELECT id FROM [User] WHERE username = @Username";
        using var selectCommand = new SqlCommand(selectUserIdQuery, connection);
        selectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
        object? userIdResult = await selectCommand.ExecuteScalarAsync();
        if (userIdResult == null || userIdResult == DBNull.Value) return true;

        const string insertQuery = "INSERT INTO [RefreshToken] (token, userid, expiration) VALUES (@Token, @UserId, @Expiration);";
        using var insertCommand = new SqlCommand(insertQuery, connection);
        insertCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = Convert.ToInt32(userIdResult);
        insertCommand.Parameters.Add("@Token", SqlDbType.VarBinary, 32).Value = token;
        insertCommand.Parameters.Add("@Expiration", SqlDbType.DateTime).Value = expiration;

        int i = await insertCommand.ExecuteNonQueryAsync();
        return i > 0;
    }
}