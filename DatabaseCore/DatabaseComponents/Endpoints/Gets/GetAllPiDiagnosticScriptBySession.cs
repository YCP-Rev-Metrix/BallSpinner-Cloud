using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<PiDiagnosticScript>> GetAllPiDiagnosticScriptBySession(int sessionId)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Return a single empty obj if sessionId is not specified on req
        if (sessionId <= 0)
        {
            return new List<PiDiagnosticScript> { new PiDiagnosticScript() } ;
        }

        string selectQuery = @"SELECT s.id, s.sessionId, s.time, s.motorId, s.instruction
                               FROM [Team_PI_Tables].[DiagnosticScript] s
                               WHERE s.sessionId = @sessionId
                               ORDER BY s.time ASC;";
        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@sessionId", sessionId);
        
        using var reader = await command.ExecuteReaderAsync();
        
        var diagnostics = new List<PiDiagnosticScript>();
        while (await reader.ReadAsync())
        {
            var d = new PiDiagnosticScript()
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                SessionId = reader["sessionId"] != DBNull.Value ? Convert.ToInt32(reader["sessionId"]) : 0,
                Time = reader["time"] != DBNull.Value ? reader["time"] is float ? Convert.ToSingle(reader["time"]) : Convert.ToSingle(Convert.ToDouble(reader["time"])) : 0.0f,
                MotorId = reader["motorId"] != DBNull.Value ? Convert.ToInt32(reader["motorId"]) : 0,
                Instruction = reader["instruction"] != DBNull.Value ? reader["instruction"] is float ? Convert.ToSingle(reader["instruction"]) : Convert.ToSingle(Convert.ToDouble(reader["instruction"])) : 0.0f,
            };
            diagnostics.Add(d);
        }

        return diagnostics;
    }
}