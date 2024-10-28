using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> Startframe(int game_id, int shot_number, int score)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string InsertQuery = "INSERT INTO [Frame] (game_id, shot_number, score)" + "VALUES (@Game_id, @Shot_number, @Score)";

        using var command = new SqlCommand(InsertQuery, connection);
        command.Parameters.Add("@Game_id", SqlDbType.BigInt).Value = game_id;
        command.Parameters.Add("@Shot_number", SqlDbType.BigInt).Value = shot_number;
        command.Parameters.Add("@Score", SqlDbType.BigInt).Value = score;

        int i = await command.ExecuteNonQueryAsync();
        return i != -1;

    }
}