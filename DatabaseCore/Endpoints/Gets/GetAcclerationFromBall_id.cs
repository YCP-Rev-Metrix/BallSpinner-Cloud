using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<(bool success, List<Shot> accelerations )> GetAcclerationFromBall_id(int ball_id)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ddx, ddy, ddz, FROM [Shot] WHERE ball_id = @Ball_id";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@Ball_id", SqlDbType.VarChar, 255).Value = ball_id;


        var accelerations = new List<Shot>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            var acceleration = new Shot
            {
                Ddx = reader.GetFloat(reader.GetOrdinal("ddx")),
                Ddy = reader.GetFloat(reader.GetOrdinal("ddy")),
                Ddz = reader.GetFloat(reader.GetOrdinal("ddz")),
            };

            accelerations.Add(acceleration);
        }
        
        return (accelerations.Any(), accelerations);
        
    }
}