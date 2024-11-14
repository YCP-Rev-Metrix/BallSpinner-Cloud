using Common.POCOs;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> InsertSimulatedShot(SimulatedShot simulatedShot, string? username)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        string insertQuery;

        // Get User Id 
        int userid = await GetUserId(username);
        
        // Create Simulated Shot Entry
        insertQuery = "INSERT INTO [SimulatedShot] (name, speed, angle, position)" +
                      "VALUES (@name, @speed, @angle, @position)" +
                      "SELECT SCOPE_IDENTITY()";
        using var insertshot = new SqlCommand(insertQuery, connection);
        insertshot.Parameters.AddWithValue("@name", simulatedShot.Name);
        insertshot.Parameters.AddWithValue("@speed", simulatedShot.Speed);
        insertshot.Parameters.AddWithValue("@angle", simulatedShot.Angle);
        insertshot.Parameters.AddWithValue("@position", simulatedShot.Position);
        int shotid =  (int)insertshot.ExecuteScalar();

        // Use the Simulated Shot Id from previous query to insert into simulated shot list 
        insertQuery = "INSERT INTO [SimulatedShotList](shotid, userid)" +
                      "VALUES (@shotid, @userid)";
        using var insertshotllist = new SqlCommand(insertQuery, connection);
        insertshotllist.Parameters.AddWithValue("@shotid", shotid);
        insertshotllist.Parameters.AddWithValue("@userid", userid);
        insertshotllist.ExecuteNonQuery();
        
        // Set up the SmartDot sensors with proper frequencies. 

        string[] sensor_name = {"Lightsensor","Accelerometer", "Gyroscope", "Magnetometer"};
        for (int x = 0; x < sensor_name.Length; x++)
        {
            insertQuery = "INSERT INTO [SD_Sensor] (frequency,sensor_type, shotid)" +
                          "VALUES (@frequency, @sensor_type, @shotid)";
            using var insertsensor = new SqlCommand(insertQuery, connection);
            insertsensor.Parameters.AddWithValue("@sample_frequency", simulatedShot.Frequecny);
            insertsensor.Parameters.AddWithValue("@sensor_type", sensor_name[x]);
            insertsensor.Parameters.AddWithValue("@shotid",shotid);
            await insertsensor.ExecuteNonQueryAsync();
        }
        
        return true;

    }
}