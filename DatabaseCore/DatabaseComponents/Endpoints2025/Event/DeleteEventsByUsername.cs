using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> DeleteEventsByUsername(string? username)
    {
        int userId = await GetUserId(username);
        if (userId <= 0) return false;
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString)) return false;
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        const string sql = "DELETE FROM combinedDB.[Events] WHERE userId = @userId;";
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@userId", userId);
        await cmd.ExecuteNonQueryAsync();
        return true;
    }
}
