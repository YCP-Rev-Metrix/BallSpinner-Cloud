using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<PiShot>> GetAllPiShotBySession(int sessionId)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Return a single empty obj if sessionId is less than or equal to 0
        if (sessionId <= 0)
        {
            return new List<PiShot> { new PiShot() } ;
        }

        string selectQuery = @"SELECT s.id, s.sessionId, s.time, s.rpm, s.angleDegrees, s.tiltDegrees
                               FROM [Team_PI_Tables].[ShotScript] s
                               WHERE s.sessionId = @sessionId
                               ORDER BY s.time ASC;";
        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@sessionId", sessionId);
        
        using var reader = await command.ExecuteReaderAsync();
        
        var shots = new List<PiShot>();
        while (await reader.ReadAsync())
        {
            var shot = new PiShot
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                SessionId = reader["sessionId"] != DBNull.Value ? Convert.ToInt32(reader["sessionId"]) : 0,
                Time = reader["time"] != DBNull.Value ? reader["time"] is float ? Convert.ToSingle(reader["time"]) : Convert.ToSingle(Convert.ToDouble(reader["time"])) : 0.0f,
                Rpm = reader["rpm"] != DBNull.Value ? Convert.ToInt32(reader["rpm"]) : 0,
                AngleDegrees = reader["angleDegrees"] != DBNull.Value ? reader["angleDegrees"] is float ? Convert.ToSingle(reader["angleDegrees"]) : Convert.ToSingle(Convert.ToDouble(reader["angleDegrees"])) : 0.0f,
                TiltDegrees = reader["tiltDegrees"] != DBNull.Value ? reader["tiltDegrees"] is float ? Convert.ToSingle(reader["tiltDegrees"]) : Convert.ToSingle(Convert.ToDouble(reader["tiltDegrees"])) : 0.0f,
            };
            shots.Add(shot);
        }

        return shots;
    }
}