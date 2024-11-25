using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> RemoveUser(string username)
    {
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string deleteQuery = "DELETE FROM [User] WHERE username = @Username";

        using var command = new SqlCommand(deleteQuery, connection);
        command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

        int rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }
}