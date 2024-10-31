using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> AddBall(string name, string weight, float hardness, string coretype, string username)
    {
        // Look into user table, get id that matches with username
        // add row to token table (token, id, expiration)
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string insertQuery = "INSERT INTO [Ball] (name, weight, hardness, coretype) " +
                             "VALUES (@name, @weight, @hardness, @coretype);";

        using var command = new SqlCommand(insertQuery, connection);
        // Set the parameter values
        command.Parameters.Add("@name", SqlDbType.VarChar).Value = username;
        command.Parameters.Add("@weight", SqlDbType.VarChar, 32).Value = weight;
        command.Parameters.Add("@hardness", SqlDbType.VarChar).Value = hardness;
        command.Parameters.Add("@coretype", SqlDbType.VarChar).Value = coretype;

        // Execute the query
        int i = await command.ExecuteNonQueryAsync();
        if (i != -1)
        {
            
        }
        return i != -1;
    }
}