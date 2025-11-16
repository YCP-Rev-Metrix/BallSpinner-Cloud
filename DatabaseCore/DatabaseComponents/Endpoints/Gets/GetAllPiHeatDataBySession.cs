using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<PiHeatData>> GetAllPiHeatDataBySession(int sessionId)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        
        // Return a single empty obj if sessionId is not specified on req
        if (sessionId <= 0)
        {
            return new List<PiHeatData> { new PiHeatData() } ;
        }
        
        string selectQuery = @"SELECT h.id, h.sessionId, h.time, h.value, h.motorId
                               FROM [Team_PI_Tables].[HeatData] h
                               WHERE h.sessionId = @sessionId
                               ORDER BY h.time ASC;";
        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@sessionId", sessionId);
        
        using var reader = await command.ExecuteReaderAsync();
        var heatDataList = new List<PiHeatData>();
        while (await reader.ReadAsync())
        {
            var h = new PiHeatData()
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                SessionId = reader["sessionId"] != DBNull.Value ? Convert.ToInt32(reader["sessionId"]) : 0,
                Time = reader["time"] != DBNull.Value
                    ? reader["time"] is float
                        ? Convert.ToSingle(reader["time"])
                        : Convert.ToSingle(Convert.ToDouble(reader["time"]))
                    : 0.0f,
                Value = reader["value"] != DBNull.Value
                    ? reader["value"] is float
                        ? Convert.ToSingle(reader["value"])
                        : Convert.ToSingle(Convert.ToDouble(reader["value"]))
                    : 0.0f,
                MotorId = reader["motorId"] != DBNull.Value ? Convert.ToInt32(reader["motorId"]) : 0,
            };
            heatDataList.Add(h);
        }

        return heatDataList;
    }
}