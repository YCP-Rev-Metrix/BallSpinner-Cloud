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

        string selectQuery = "SELECT id FROM [User] WHERE username = @Username"; // Adjusted to select more fields
        
        using var command = new SqlCommand(selectQuery, connection);
        
        command.Parameters.AddWithValue("@Username", username);
            
        var result = command.ExecuteScalarAsync();
        return result.Result == DBNull.Value ? 0 : Convert.ToInt32(result.Result);
    }
}