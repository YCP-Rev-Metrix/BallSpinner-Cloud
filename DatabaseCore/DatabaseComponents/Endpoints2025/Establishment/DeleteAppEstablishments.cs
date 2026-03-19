using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    /// <summary>
    /// Deletes all rows from Establishments (when called after Sessions are deleted, we cannot scope by user).
    /// Used by "replace cloud with local" to clear then repopulate from local.
    /// </summary>
    public async Task<bool> DeleteAppEstablishments(string? username, int? mobileID)
    {
        int userId = await GetUserId(username, mobileID);
        if (userId <= 0) return false;
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString)) return false;
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        const string sql = "DELETE FROM combinedDB.[Establishments];";
        using var cmd = new SqlCommand(sql, connection);
        await cmd.ExecuteNonQueryAsync();
        return true;
    }
}
