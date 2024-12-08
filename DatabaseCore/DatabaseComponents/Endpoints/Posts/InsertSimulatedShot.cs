using System.Data;
using System.Text;
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
                string insertQuery = "INSERT INTO [SimulatedShot] (speed, angle, position, Created) " +
                                     "VALUES (@speed, @angle, @position, @Created)" +
                                     "SELECT SCOPE_IDENTITY()";
                using var insertShot = new SqlCommand(insertQuery, connection);
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
                insertQuery = "INSERT INTO [SimulatedShotList](shotid, userid, name) VALUES (@shotid, @userid, @name)";
                using var insertShotList = new SqlCommand(insertQuery, connection);
                insertShotList.Parameters.AddWithValue("@shotid", shotid);
                insertShotList.Parameters.AddWithValue("@userid", userid);
                insertShotList.Parameters.AddWithValue("@name", shot.simulatedShot.Name);

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
                // Batch query
                var query = new StringBuilder(@"
                INSERT INTO [SensorData] 
                (sensor_id, count, brightness, xaxis, yaxis, zaxis, waxis, logtime) 
                VALUES ");

                var parameters = new List<SqlParameter>();
                int parameterIndex = 0;

                foreach (SampleData data in shot.data)
                {
                    long sensorid = data.Type switch
                    {
                        "1" => sensorsCreated[0],
                        "2" => sensorsCreated[1],
                        "3" => sensorsCreated[2],
                        "4" => sensorsCreated[3],
                        _ => throw new ArgumentOutOfRangeException(nameof(data.Type), "Invalid sensor type")
                    };

                    // Add placeholders for each row
                    query.Append($"(@sensor_id{parameterIndex}, @count{parameterIndex}, @brightness{parameterIndex}, ");
                    query.Append($"@xaxis{parameterIndex}, @yaxis{parameterIndex}, @zaxis{parameterIndex}, ");
                    query.Append($"@waxis{parameterIndex}, @logtime{parameterIndex}),");

                    // Add parameters for the row
                    parameters.Add(new SqlParameter($"@sensor_id{parameterIndex}", sensorid));
                    parameters.Add(new SqlParameter($"@count{parameterIndex}", data.Count ?? (object)DBNull.Value));

                    // Explicitly handle brightness
                    parameters.Add(new SqlParameter($"@brightness{parameterIndex}",data.Brightness?? (object)DBNull.Value)); 

                    parameters.Add(new SqlParameter($"@xaxis{parameterIndex}", data.X ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter($"@yaxis{parameterIndex}", data.Y ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter($"@zaxis{parameterIndex}", data.Z ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter($"@waxis{parameterIndex}", DBNull.Value));

                    parameters.Add(new SqlParameter($"@logtime{parameterIndex}", data.Logtime ?? (object)DBNull.Value));

                    parameterIndex++;
                }

                // Remove the trailing comma
                if (query.Length > 0 && query[query.Length - 1] == ',')
                {
                    query.Length--; 
                }


                // Execute the batch query
                using var command = new SqlCommand(query.ToString(), connection);
                command.Parameters.AddRange(parameters.ToArray());
                await command.ExecuteNonQueryAsync();
                
                return true; 
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exceptions
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

