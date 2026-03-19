using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> DeleteAppShots(string? username, int? mobileID)
    {
        int userId = await GetUserId(username, mobileID);
        if (userId <= 0) return false;
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString)) return false;
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        const string sql = @"
            DELETE FROM combinedDB.[Shots]
            WHERE SessionID IN (SELECT ID FROM combinedDB.[Sessions] WHERE EventID IN (SELECT Id FROM combinedDB.[Events] WHERE userId = @userId))
               OR BallID IN (SELECT id FROM combinedDB.[Balls] WHERE userId = @userId);";
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@userId", userId);
        await cmd.ExecuteNonQueryAsync();
        return true;
    }
}
