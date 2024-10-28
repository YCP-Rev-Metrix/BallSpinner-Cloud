using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> Startgame(int session_id, int score)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        string insertQuery = "INSERT INTO [Game] (session_id, score)" +
                              " VALUES (@Session_id, @Score)";


        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@Session_id", SqlDbType.BigInt).Value = session_id;
        command.Parameters.Add("@Score", SqlDbType.BigInt).Value = score;

        int i = await command.ExecuteNonQueryAsync();
        return i != -1;
    }
}