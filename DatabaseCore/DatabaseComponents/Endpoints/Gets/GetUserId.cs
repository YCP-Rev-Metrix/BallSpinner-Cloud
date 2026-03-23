using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<int> GetUserId(string? username)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        // Prefer combinedDB users (cloud app tables reference this), then fall back to legacy [User].
        int combinedDbUserId = await TryGetUserId(connection, "[combinedDB].[Users]", username);
        if (combinedDbUserId > 0) return combinedDbUserId;
        return await TryGetUserId(connection, "[User]", username);
    }

    // Overload used by delete endpoints: match the user row by both username and mobileId when possible.
    public async Task<int> GetUserId(string? username, int? mobileID)
    {
        // Fallback to username-only if mobileID isn't provided.
        if (!mobileID.HasValue || mobileID.Value <= 0)
            return await GetUserId(username);

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Prefer combinedDB users (cloud app tables reference this), then fall back to legacy [User].
        int combinedDbUserId = await TryGetUserId(connection, "[combinedDB].[Users]", username, mobileID.Value);
        if (combinedDbUserId > 0) return combinedDbUserId;
        int legacyUserId = await TryGetUserId(connection, "[User]", username, mobileID.Value);
        if (legacyUserId > 0) return legacyUserId;
        return await GetUserId(username);
    }

    private static async Task<int> TryGetUserId(SqlConnection connection, string tableName, string? username, int? mobileID = null)
    {
        try
        {
            string selectQuery = mobileID.HasValue
                ? $"SELECT id FROM {tableName} WHERE username = @Username AND mobileId = @MobileID"
                : $"SELECT id FROM {tableName} WHERE username = @Username";

            using var command = new SqlCommand(selectQuery, connection);
            command.Parameters.AddWithValue("@Username", username);
            if (mobileID.HasValue) command.Parameters.AddWithValue("@MobileID", mobileID.Value);

            var result = await command.ExecuteScalarAsync();
            return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }
        catch (SqlException)
        {
            // Allows compatibility with environments where either table/column isn't present.
            return 0;
        }
    }
}
