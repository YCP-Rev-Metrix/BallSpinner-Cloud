using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> RemoveRelatedRefreshTokens(string username)
    {
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string deleteQuery = """
                    DELETE FROM [Token]
                    WHERE userid = (SELECT id FROM [User] WHERE username = @Username);
                    """;

        using var command = new SqlCommand(deleteQuery, connection);
        // Set the parameter values
        command.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;

        // Execute the query
        int i = await command.ExecuteNonQueryAsync();
        return i != -1;
    }
}