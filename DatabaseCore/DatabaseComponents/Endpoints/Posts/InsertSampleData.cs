using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> InsertSampleData(int? sensorType, int? count, float? timestamp, float? x, float? y, float? z)
    {
        string connectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        string insertQuery = @"
            INSERT INTO [SensorData] (sensor_id, yaxis, xaxis, zaxis, brightness, samplenumber, timestamp) 
                VALUES (@sensor_id, @yaxis, @xaxis, @zaxis, @brightness, @samplenumber, @TimeStamp)";
        
        using var command = new SqlCommand(insertQuery, connection);
        
        // Set parameters
        command.Parameters.AddWithValue("@sensor_id", sensorType);
        command.Parameters.AddWithValue("@yaxis", y);
        command.Parameters.AddWithValue("@xaxis", x);
        command.Parameters.AddWithValue("@zaxis", z);
        command.Parameters.AddWithValue("@brightness", 1.22);
        command.Parameters.AddWithValue("@samplenumber", count);
        command.Parameters.AddWithValue("@TimeStamp", timestamp);
        
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }
}