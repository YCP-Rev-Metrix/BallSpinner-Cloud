using Common.POCOs;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> InsertSampleData(SampleData sampleData)
    {
        string connectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        string insertQuery = @"
            INSERT INTO [SensorData] (sensor_id, count, xaxis, yaxis, zaxis, waxis, logtime) 
                VALUES (@sensor_id, @count, @xaxis, @yaxis, @zaxis, @waxis, @logtime)";
        
        using var command = new SqlCommand(insertQuery, connection);
        
        // Set parameters
        command.Parameters.AddWithValue("@sample_number", sampleData.Count ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@xaxis", sampleData.X ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@yaxis", sampleData.Y ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@zaxis", sampleData.Z ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@waxis", null ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@logtime", sampleData.Logtime ?? (object)DBNull.Value);
        
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }
}