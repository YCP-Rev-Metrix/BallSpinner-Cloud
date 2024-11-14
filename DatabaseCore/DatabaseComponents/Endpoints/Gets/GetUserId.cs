using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<int> GetUserId(string? username)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT id FROM [User] WHERE username = @Username"; // Adjusted to select more fields
        
        using var command = new SqlCommand(selectQuery, connection);
        
        command.Parameters.AddWithValue("@Username", username);
            
        object result = command.ExecuteScalarAsync();
        if (int.TryParse(result.ToString(), out int userId))
        {
            return userId;
        }

        // If result is null or not an integer, handle accordingly
        throw new InvalidOperationException($"User not found or invalid user ID.\n Username: {username} \n object: {result}");
    }
}