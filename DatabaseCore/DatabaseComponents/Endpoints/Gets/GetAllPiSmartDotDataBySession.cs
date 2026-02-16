using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<PiSmartDotData>> GetAllPiSmartDotDataBySession(int sessionId)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Return a single empty obj if sessionId is less than or equal to 0
        if (sessionId <= 0)
        {
            return new List<PiSmartDotData> { new PiSmartDotData() } ;
        }

        string selectQuery = @"SELECT
        s.id, s.sessionId, s.time, s.dataSelector, s.accelX, s.accelY, s.accelZ,
        s.gyroX, s.gyroY, s.gyroZ, s.magnoX, s.magnoY, s.magnoZ, s.light, s.replayIteration
        FROM [Team_PI_Tables].[SmartDotData] s
        WHERE s.sessionId = @sessionId
        ORDER BY s.time ASC;";
        
        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@sessionId", sessionId);
        
        using var reader = await command.ExecuteReaderAsync();
        
        var smartDotDataList = new List<PiSmartDotData>();
        while (await reader.ReadAsync())
        {
            var data = new PiSmartDotData
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                SessionId = reader["sessionId"] != DBNull.Value ? Convert.ToInt32(reader["sessionId"]) : 0,
                Time = reader["time"] != DBNull.Value
                    ? reader["time"] is float
                        ? Convert.ToSingle(reader["time"])
                        : Convert.ToSingle(Convert.ToDouble(reader["time"]))
                    : 0.0f,
                DataSelector = reader["dataSelector"] != DBNull.Value ? Convert.ToInt32(reader["dataSelector"]) : 0,
                XL_X = reader["accelX"] != DBNull.Value
                    ? reader["accelX"] is float
                        ? Convert.ToSingle(reader["accelX"])
                        : Convert.ToSingle(Convert.ToDouble(reader["accelX"]))
                    : 0.0f,
                XL_Y = reader["accelY"] != DBNull.Value
                    ? reader["accelY"] is float
                        ? Convert.ToSingle(reader["accelY"])
                        : Convert.ToSingle(Convert.ToDouble(reader["accelY"]))
                    : 0.0f,
                XL_Z = reader["accelZ"] != DBNull.Value
                    ? reader["accelZ"] is float
                        ? Convert.ToSingle(reader["accelZ"])
                        : Convert.ToSingle(Convert.ToDouble(reader["accelZ"]))
                    : 0.0f,
                GY_X = reader["gyroX"] != DBNull.Value
                    ? reader["gyroX"] is float
                        ? Convert.ToSingle(reader["gyroX"])
                        : Convert.ToSingle(Convert.ToDouble(reader["gyroX"]))
                    : 0.0f,
                GY_Y = reader["gyroY"] != DBNull.Value
                    ? reader["gyroY"] is float
                        ? Convert.ToSingle(reader["gyroY"])
                        : Convert.ToSingle(Convert.ToDouble(reader["gyroY"]))
                    : 0.0f,
                GY_Z = reader["gyroZ"] != DBNull.Value
                    ? reader["gyroZ"] is float
                        ? Convert.ToSingle(reader["gyroZ"])
                        : Convert.ToSingle(Convert.ToDouble(reader["gyroZ"]))
                    : 0.0f,
                MG_X = reader["magnoX"] != DBNull.Value
                    ? reader["magnoX"] is float
                        ? Convert.ToSingle(reader["magnoX"])
                        : Convert.ToSingle(Convert.ToDouble(reader["magnoX"]))
                    : 0.0f,
                MG_Y = reader["magnoY"] != DBNull.Value
                    ? reader["magnoY"] is float
                        ? Convert.ToSingle(reader["magnoY"])
                        : Convert.ToSingle(Convert.ToDouble(reader["magnoY"]))
                    : 0.0f,
                MG_Z = reader["magnoZ"] != DBNull.Value
                    ? reader["magnoZ"] is float
                        ? Convert.ToSingle(reader["magnoZ"])
                        : Convert.ToSingle(Convert.ToDouble(reader["magnoZ"]))
                    : 0.0f,
                LT = reader["light"] != DBNull.Value
                    ? reader["light"] is float
                        ? Convert.ToSingle(reader["light"])
                        : Convert.ToSingle(Convert.ToDouble(reader["light"]))
                        : 0.0f,
                ReplayIteration = reader["replayIteration"] != DBNull.Value ? Convert.ToInt32(reader["replayIteration"]) : 0
            };
            smartDotDataList.Add(data);
        }
        
        return smartDotDataList;
    }
}