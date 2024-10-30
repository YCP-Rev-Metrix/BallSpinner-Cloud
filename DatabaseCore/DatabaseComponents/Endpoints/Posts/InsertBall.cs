using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> Insertball(float weight, string? color)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        string insertQuery = "INSERT INTO [Ball] (weight , color) " +
                                "VALUES (@Weight, @Color)";
        using var command = new SqlCommand(insertQuery, connection);

        command.Parameters.Add("@Weight", SqlDbType.Float).Value = weight;
        // Using ball to test the null 
        if (color != null)
        {
            command.Parameters.Add("@Color", SqlDbType.VarChar).Value = color;
        }
        else
        {
            command.Parameters.Add("@Color", SqlDbType.VarChar).Value = DBNull.Value;
        }


        int i = await command.ExecuteNonQueryAsync();
        return i != -1;

    }
}