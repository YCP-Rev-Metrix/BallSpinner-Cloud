using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents
{
    public partial class RevMetrixDb
    {
        public async Task<bool> InsertSimulatedShot(SimulatedShot shot, string? username)
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
                await using var connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();

                // Get User Id
                int userid = await GetUserId(username);
                if (userid <= 0)
                {
                    throw new ArgumentException("Invalid user ID.");
                }

                // Create Simulated Shot Entry
                string insertQuery = "INSERT INTO [SimulatedShot] (name, speed, angle, position, Created) " +
                                     "VALUES (@name, @speed, @angle, @position, @Created)" +
                                     "SELECT SCOPE_IDENTITY()";
                using var insertShot = new SqlCommand(insertQuery, connection);
                insertShot.Parameters.AddWithValue("@name", shot.simulatedShot.Name);
                insertShot.Parameters.AddWithValue("@speed", shot.simulatedShot.Speed);
                insertShot.Parameters.AddWithValue("@angle", shot.simulatedShot.Angle);
                insertShot.Parameters.AddWithValue("@position", shot.simulatedShot.Position);
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
                long[] sensorIds = { 1, 2, 3, 4 };
                long[] sensorsCreated = new long[4];
                int i = 0;
                foreach (var id in sensorIds)
                {
                    insertQuery = @"INSERT INTO [SD_Sensor] (frequency, type_id, shotid)
                                    OUTPUT INSERTED.sensor_id
                                    VALUES (@frequency, @typeid, @shotid)";
                    using var insertSensor = new SqlCommand(insertQuery, connection);
                    insertSensor.Parameters.AddWithValue("@frequency", shot.simulatedShot.Frequency);
                    insertSensor.Parameters.AddWithValue("@typeid", id);
                    insertSensor.Parameters.AddWithValue("@shotid", shotid);
                    object? result = await insertSensor.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                    {
                        throw new InvalidOperationException("Failed to insert sensor or retrieve sensorid ID.");
                    }

                    int sensorId = Convert.ToInt32(result);
                    sensorsCreated[i] = sensorId;
                    i++;
                }
                
                i = 0;

                foreach (SampleData data in shot.data)
                {
                    long sensorid = data.Type switch
                    {
                        "1" => sensorsCreated[0],
                        "2" => sensorsCreated[1],
                        "3" => sensorsCreated[2],
                        "4" => sensorsCreated[3],
                        _ => throw new ArgumentOutOfRangeException(nameof(shot))
                    };
                    insertQuery = @"
                        INSERT INTO [SensorData] (sensor_id, count, brightness, xaxis, yaxis, zaxis, waxis, logtime) 
                        VALUES (@sensor_id, @count, @brightness, @xaxis, @yaxis, @zaxis, NULL, @logtime)";

                    using var command = new SqlCommand(insertQuery, connection);

                    // Set parameters
                    command.Parameters.AddWithValue("@sensor_id", sensorid);
                    command.Parameters.AddWithValue("@count", data.Count ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@brightness", 0);
                    command.Parameters.AddWithValue("@xaxis", data.X ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@yaxis", data.Y ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@zaxis", data.Z ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@waxis", data.W ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@logtime", data.Logtime ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                    i++;
                }

                return true; // Operation succeeded
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

