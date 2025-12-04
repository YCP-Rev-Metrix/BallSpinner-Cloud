using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<PiEncoderData>> GetAllPiEncoderDataBySession(int sessionId)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Return a single empty obj if sessionId is not specified on req
        if (sessionId <= 0)
        {
            return new List<PiEncoderData> { new PiEncoderData() } ;
        }
        
        string selectQuery = @"SELECT e.id, e.sessionId, e.time, e.pulses, e.motorId, e.replayIteration
                               FROM [Team_PI_Tables].[EncoderData] e
                               WHERE e.sessionId = @sessionId
                               ORDER BY e.time ASC;";
        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@sessionId", sessionId);
        
        using var reader = await command.ExecuteReaderAsync();
        
        var encoderDataList = new List<PiEncoderData>();
        while (await reader.ReadAsync())
        {
            var e = new PiEncoderData()
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                SessionId = reader["sessionId"] != DBNull.Value ? Convert.ToInt32(reader["sessionId"]) : 0,
                Time = reader["time"] != DBNull.Value
                    ? reader["time"] is float
                        ? Convert.ToSingle(reader["time"])
                        : Convert.ToSingle(Convert.ToDouble(reader["time"]))
                    : 0.0f,
                Pulses = reader["pulses"] != DBNull.Value ? Convert.ToInt32(reader["pulses"]) : 0,
                MotorId = reader["motorId"] != DBNull.Value ? Convert.ToInt32(reader["motorId"]) : 0,
                ReplayIteration = reader["replayIteration"] != DBNull.Value ? Convert.ToInt32(reader["replayIteration"]) : 0
            };
            encoderDataList.Add(e);
        }
        
        return encoderDataList;
    }
}