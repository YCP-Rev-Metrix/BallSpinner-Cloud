using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<(bool success, List<Shot> users)> GetStrikesVsShotsPercentage(int? user_id)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT user_id, " +
                             "(SUM(CASE WHEN pins_remaining = 0 THEN 1 ELSE 0 END) * 100.0 / COUNT(*)) AS strike_percentage," +
                             "(SUM(CASE WHEN pins_remaining <> 0 THEN 1 ELSE 0 END) * 100.0 / COUNT(*)) AS shot_percentage" +
                             "FROM (SELECT TOP 100 user_id, pins_remaining   FROM Shot  WHERE user_id = @User_id  ORDER BY frame_id DESC) " +
                             "AS last_100_frames GROUP BY user_id;"; 

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@User_id", SqlDbType.VarChar, 255).Value = user_id;


        var shots = new List<Shot>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        

        while (await reader.ReadAsync())
        {
            var percentage = new Shot
            {
                Shot_id = reader.GetInt32(reader.GetOrdinal("shot_id")),
                //Strike_percentage = reader.GetInt32(reader.GetOrdinal("strike_percentage")),
                //Shot_percentage = reader.GetInt32(reader.GetOrdinal("shot_percentage")),
            };

            shots.Add(percentage);
        }

        return (shots.Any(), shots);
    }
}