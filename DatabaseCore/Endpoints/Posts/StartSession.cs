using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    // This may work, still need to create one of the foreign keys
    public async Task<bool> Startsession(int? leauge_id, int? tournament_id, int? practice_id, DateTime date, string location, int totalgames, int totalframes)
    {
        // If all 3 are null then session can't start
        if (leauge_id > -1 && tournament_id > -1 && practice_id > -1)
        {
            return false;
        }

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string startsessionQuery = "INSERT INTO [Session] (league_id, tournament_id, practice_id, date, location, total_games, total_frames)" +
                                    "VALUES (@League_id, @Tournament_id, @Practice_id, @Date, @Location, @Total_games, @Total_frames)";
        using var command = new SqlCommand(startsessionQuery, connection);

        // If leauge_id is null
        if (leauge_id > -1)
        {
            command.Parameters.Add("@League_id", SqlDbType.BigInt).Value = leauge_id;
        }
        else
        {
            command.Parameters.Add("@League_id", SqlDbType.BigInt).Value = DBNull.Value;
        }
        // If tournament_id is null
        if (tournament_id > -1)
        {
            command.Parameters.Add("@Tournament_id", SqlDbType.BigInt).Value = tournament_id;
        }
        else
        {
            command.Parameters.Add("@Tournament_id", SqlDbType.BigInt).Value = DBNull.Value;
        }
        // If practice_id is null
        if (practice_id > -1)
        {
            command.Parameters.Add("@Practice_id", SqlDbType.BigInt).Value = practice_id;
        }
        else
        {
            command.Parameters.Add("@Practice_id", SqlDbType.BigInt).Value = DBNull.Value;
        }

        command.Parameters.Add("@Date", SqlDbType.DateTime, 2).Value = date;
        command.Parameters.Add("@Location", SqlDbType.VarChar, 255).Value = location;
        command.Parameters.Add("@Total_games", SqlDbType.BigInt).Value = totalgames;
        command.Parameters.Add("@Total_frames", SqlDbType.BigInt).Value = totalframes;
        int i = await command.ExecuteNonQueryAsync();
        return i != -1;
    }
}