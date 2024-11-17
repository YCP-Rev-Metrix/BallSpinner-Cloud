using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents
{
    public partial class RevMetrixDB
    {
        public async Task<bool> InsertSimulatedShot(Shot shot, string? username)
        {
            try
            {
                // Validate connection string
                ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new InvalidOperationException("Connection string is not set.");
                }

                // Open the connection
                using var connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();

                // Get User Id
                int userid = await GetUserId(username);
                if (userid <= 0)
                {
                    throw new ArgumentException("Invalid user ID.");
                }

                // Create Simulated Shot Entry
                string insertQuery = "INSERT INTO [SimulatedShot] (name, speed, angle, position, Created) " +
                                     "VALUES (@name, @speed, @angle, @position, @Created); " +
                                     "SELECT SCOPE_IDENTITY()";
                using var insertShot = new SqlCommand(insertQuery, connection);
                insertShot.Parameters.AddWithValue("@name", shot.SimulatedShot.Name);
                insertShot.Parameters.AddWithValue("@speed", shot.SimulatedShot.Speed);
                insertShot.Parameters.AddWithValue("@angle", shot.SimulatedShot.Angle);
                insertShot.Parameters.AddWithValue("@position", shot.SimulatedShot.Position);
                insertShot.Parameters.AddWithValue("@Created", DateTime.Now);

                var shotResult = await insertShot.ExecuteScalarAsync();

                // Parse shot id into integer and handle null results
                if (shotResult == null || shotResult == DBNull.Value)
                {
                    throw new InvalidOperationException("Failed to insert simulated shot or retrieve shot ID.");
                }

                int shotid = Convert.ToInt32(shotResult);

                // Use the Simulated Shot Id from the previous query to insert into simulated shot list
                insertQuery = "INSERT INTO [SimulatedShotList](shotid, userid) VALUES (@shotid, @userid)";
                using var insertShotList = new SqlCommand(insertQuery, connection);
                insertShotList.Parameters.AddWithValue("@shotid", shotid);
                insertShotList.Parameters.AddWithValue("@userid", userid);
                await insertShotList.ExecuteNonQueryAsync();

                // Set up the SmartDot sensors with proper frequencies
                string[] sensorNames = { "Lightsensor", "Accelerometer", "Gyroscope", "Magnetometer" };
                long[] sensorIds = new long[4];
                int i = 0;
                foreach (var sensorName in sensorNames)
                {
                    insertQuery = "INSERT INTO [SD_Sensor] (frequency, type, shotid)" +
                                  "VALUES (@frequency, @type, @shotid)";
                    using var insertSensor = new SqlCommand(insertQuery, connection);
                    insertSensor.Parameters.AddWithValue("@frequency", shot.SimulatedShot.Frequency);
                    insertSensor.Parameters.AddWithValue("@type", sensorName);
                    insertSensor.Parameters.AddWithValue("@shotid", shotid);
                    await insertSensor.ExecuteNonQueryAsync();
                }

                i = 0;
                
                foreach (var sensorName in sensorNames)
                {
                    // Use parameterized query to prevent SQL injection and ensure proper handling of values
                    insertQuery = "SELECT sensor_id FROM [SD_Sensor] WHERE shotid = @shotid AND type = @sensorName";

                    using var insertSensor = new SqlCommand(insertQuery, connection);

                    // Add parameters to the SQL command to prevent SQL injection
                    insertSensor.Parameters.AddWithValue("@shotid", shotid);
                    insertSensor.Parameters.AddWithValue("@sensorName", sensorName);

                    var sensorId = await insertSensor.ExecuteScalarAsync();
                   
                    sensorIds[i] = (long)sensorId;
                    i++;
                }

                foreach (SampleData data in shot.Data)
                {
                    long sensorid;
                    if (data.Type == sensorNames[0])
                    {
                        sensorid = sensorIds[0];
                    }
                    else if (data.Type == sensorNames[1])
                    {
                        sensorid = sensorIds[1];
                    }
                    else if (data.Type == sensorNames[2])
                    {
                        sensorid = sensorIds[2];
                    }
                    else
                    {
                        sensorid = sensorIds[3];
                    }
                    
                    insertQuery = @"
                        INSERT INTO [SensorData] (sensor_id, count, brightness, xaxis, yaxis, zaxis, waxis, logtime) 
                        VALUES (@sensor_id, @count, @brightness, @xaxis, @yaxis, @zaxis, @waxis, @logtime)";
        
                    using var command = new SqlCommand(insertQuery, connection);
        
                    // Set parameters
                    command.Parameters.AddWithValue("@sensor_id", sensorid);
                    command.Parameters.AddWithValue("@count", data.Count ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@brightness", 0);
                    command.Parameters.AddWithValue("@xaxis", data.X ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@yaxis", data.Y ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@zaxis", data.Z ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@waxis", data.W ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@logtime", data.Logtime ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }

                return true;  // Operation succeeded
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exceptions (e.g., connection issues, query errors)
                LogWriter.LogInfo(sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Log general exceptions
                LogWriter.LogError(ex.Message);
                return false;
            }
        }
    }
}
