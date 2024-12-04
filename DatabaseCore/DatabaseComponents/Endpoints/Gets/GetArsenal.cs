using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;


namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<Arsenal> GetArsenalbyUsername(string? username)
    {
        var arsenal = new Arsenal();
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = @"
        SELECT b.name, b.core_type, b.diameter, b.weight, a.status
        FROM [User] AS u
        INNER JOIN Arsenal AS a ON u.id = a.userid
        INNER JOIN Ball AS b ON a.ball_id = b.ballid
        WHERE u.username = @Username AND a.status = 1;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@Username", username);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        { 
            
            var name = reader["name"].ToString();
            var weight = reader.GetNullableValue<double>("weight");
            var diameter = reader. GetNullableValue<double>("diameter");
            string? coreType = reader["core_type"].ToString();

            var ball = new Ball(name, diameter, weight, coreType);
            arsenal.BallList.Add(ball);
        }
        return arsenal;
    }


}

