using System.Data;
using Common.POCOs;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> InsertSimulatedShot(SimulatedShot simulatedShot)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        string insertQuery;
        int i;
        
        insertQuery = "INSERT INTO [SimulatedShot] (name, speed, angle, position)" +
                      "VALUES (@name, @speed, @angle, @position)" +
                      "SELECT SCOPE_IDENTITY()";
        using var insertshot = new SqlCommand(insertQuery, connection);
        insertshot.Parameters.AddWithValue("@name", simulatedShot.Name);
        insertshot.Parameters.AddWithValue("@speed", simulatedShot.Speed);
        insertshot.Parameters.AddWithValue("@angle", simulatedShot.Angle);
        insertshot.Parameters.AddWithValue("@position", simulatedShot.Position);
        var success = await insertshot.ExecuteScalarAsync();
        
        // Set up the SmartDot sensors with proper frequencies. 

        string[] sensor_name = {"Lightsensor","Accelerometer", "Gyroscope", "Magnetometer"};
        for (int x = 0; x < sensor_name.Length; x++)
        {
            insertQuery = "INSERT INTO [SD_Sensor] (frequency,sensor_type, shotid)" +
                          "VALUES (@frequency, @sensor_type, @shotid)";
            using var insertsensor = new SqlCommand(insertQuery, connection);
            insertsensor.Parameters.AddWithValue("@sample_frequency", simulatedShot.Frequecny);
            insertsensor.Parameters.AddWithValue("@sensor_type", sensor_name[x]);
            insertsensor.Parameters.AddWithValue("@shotid",success);
            await insertsensor.ExecuteNonQueryAsync();
        }
        
        return true;

    }
}