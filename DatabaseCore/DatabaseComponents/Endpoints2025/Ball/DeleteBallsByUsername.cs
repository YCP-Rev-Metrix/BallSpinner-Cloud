using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> DeleteBallsByUsername(string? username, int? mobileID)
    {
        int userId = await GetUserId(username, mobileID);
        if (userId <= 0) return false;
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString)) return false;
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        const string sql = "DELETE FROM combinedDB.[Balls] WHERE userId = @userId;";
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@userId", userId);
        await cmd.ExecuteNonQueryAsync();
        return true;
    }
}
