using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;
using System.CodeDom.Compiler;

namespace DatabaseCore.DatabaseComponents.Endpoints.Puts;

public partial class RevMetrixDb
{
    public async Task<bool> UpdateBall(Common.POCOs.Ball ball)
    {
        string ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string updateQuery = "UPDATE [Ball] SET diameter = @Diameter, weight = @Weight, coretype = @CoreType WHERE id = @Id";

        using var command = new SqlCommand(updateQuery, connection);
        command.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = ball.Name ?? (object)DBNull.Value;
        command.Parameters.Add("@Diameter", SqlDbType.Float).Value = ball.Diameter ?? (object)DBNull.Value;
        command.Parameters.Add("@Weight", SqlDbType.Float).Value = ball.Weight ?? (object)DBNull.Value;
        command.Parameters.Add("@CoreType", SqlDbType.VarChar, 255).Value = ball.CoreType ?? (object)DBNull.Value;

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}