using System.Data;
using System.Text;
using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.HadrModel;

namespace DatabaseCore.DatabaseComponents
{
    public partial class RevMetrixDb
    {
        public async Task<bool> InsertSimulatedShot(SimulatedShot shot, string? username)
        {
            try
            {
                // Declare global variables
                int sensorInfoID;
                int ballID;
                int InitialValuesID;
                int shotid;
                // Validate connection string
                ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new InvalidOperationException("Connection string is not set.");
                }

                // Open the connection
                await using var connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();
                // start transaction
                using (SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync())
                {
                    // Get User Id
                    int userid = await GetUserId(username);
                    if (userid <= 0)
                    {
                        throw new ArgumentException("Invalid user ID.");
                    }

                    // Insert initial values
                    string initialValuesQuery = @"
                    INSERT INTO InitialValues (InitialPointx, InitialPointy, InflectionPointx, InflectionPointy, FinalPointx, FinalPointy, TimeStep)
                    VALUES(@initPointx, @initPointy, @inflectionPointx, @inflectionPointy, @finalPointx, @finalPointy, @timeStep)
                    SELECT SCOPE_IDENTITY()
                    ";
                    using (var insertInitValues = new SqlCommand(initialValuesQuery, connection, transaction))
                    {
                        insertInitValues.Parameters.AddWithValue("@initPointx", shot.shotinfo.BezierInitPoint.x);
                        insertInitValues.Parameters.AddWithValue("@initPointy", shot.shotinfo.BezierInitPoint.y);
                        insertInitValues.Parameters.AddWithValue("@inflectionPointx", shot.shotinfo.BezierInflectionPoint.x);
                        insertInitValues.Parameters.AddWithValue("@inflectionPointy", shot.shotinfo.BezierInflectionPoint.y);
                        insertInitValues.Parameters.AddWithValue("@finalPointx", shot.shotinfo.BezierInitPoint.x);
                        insertInitValues.Parameters.AddWithValue("@finalPointy", shot.shotinfo.BezierInitPoint.y);
                        insertInitValues.Parameters.AddWithValue("@timeStep", shot.shotinfo.TimeStep);
                        var insertResult = await insertInitValues.ExecuteScalarAsync();
                        if (insertResult == null || insertResult == DBNull.Value)
                        {
                            throw new InvalidOperationException("Failed to insert initial values");
                        }
                        InitialValuesID = Convert.ToInt32(insertResult);
                    }

                    // Create new sensorInfo entry
                    string sensorInfoQuery = @"
                        INSERT INTO SensorInfo (SmartDotID, Date, Comments)
                        OUTPUT INSERTED.infoID
                        SELECT sl.SmartDotID, GETDATE(), @Comments
                        FROM SensorList sl
                        WHERE MACAddress = @MacAddress
                    ";
                    using (SqlCommand sensorInfo = new SqlCommand(sensorInfoQuery, connection, transaction))
                    {
                        sensorInfo.Parameters.AddWithValue("@Comments", shot.sensorInfo.Comments);
                        sensorInfo.Parameters.AddWithValue("@MacAddress", shot.sensorInfo.MACAddress);
                        var SensorInfoID = await sensorInfo.ExecuteScalarAsync();
                        if (SensorInfoID == null || SensorInfoID == DBNull.Value)
                        {
                            throw new InvalidOperationException("Failed to insert simulated shot or retrieve shot ID.");
                        }
                        sensorInfoID = Convert.ToInt32(SensorInfoID);
                    }

                    // Get ballID from the shot - FOR NOW THIS ASSUMES EACH USER ONLY HAS ONE ARSENAL. IN THE FUTURE, UPDATE THE POCO SO A USER CAN SEND A BALL FROM A SPECIFIC ARSENAL
                    string ballQuery = @"
                        SELECT b.ballid
                        FROM [User] u
                        INNER JOIN Arsenal a ON u.id = a.userid
                        INNER JOIN Ball b ON b.ArsenalID = a.arsenalID
                        WHERE b.name = @name AND u.id = @userID
                    ";
                    using (var GetBallQuery = new SqlCommand(ballQuery, connection, transaction))
                    {
                        GetBallQuery.Parameters.AddWithValue("@name", shot.ball.Name);
                        GetBallQuery.Parameters.AddWithValue("@userID", userid);
                        var ball = GetBallQuery.ExecuteScalar();
                        if (ball == null || ball == DBNull.Value)
                        {
                            throw new InvalidOperationException("Failed to retrive ball. Given ball does not exist");
                        }
                        ballID = Convert.ToInt32(ball);
                    }

                    // Create Simulated Shot Entry
                    string insertQuery = "INSERT INTO [SimulatedShot] (smartdot_sensorsid, ballid, InitialValuesID, Name, Created) " +
                        "VALUES (@sensorInfo, @ballid, @InitialValues, @Name, @Created)" +
                        "SELECT SCOPE_IDENTITY()";
                    using (var insertShot = new SqlCommand(insertQuery, connection, transaction))
                    {
                        insertShot.Parameters.AddWithValue("@sensorInfo", sensorInfoID);
                        insertShot.Parameters.AddWithValue("@ballid", ballID);
                        insertShot.Parameters.AddWithValue("@InitialValues", InitialValuesID);
                        insertShot.Parameters.AddWithValue("@Name", shot.shotinfo.Name);
                        insertShot.Parameters.AddWithValue("@Created", DateTime.Now);
                        var shotResult = await insertShot.ExecuteScalarAsync();

                        // Parse shot id into integer and handle null results
                        if (shotResult == null || shotResult == DBNull.Value)
                        {
                            throw new InvalidOperationException("Failed to insert simulated shot or retrieve shot ID.");
                        }

                        shotid = Convert.ToInt32(shotResult);
                    }

                    // Use the Simulated Shot Id from the previous query to insert into simulated shot list
                    insertQuery = "INSERT INTO [SimulatedShotList](shotid, userid, name) VALUES (@shotid, @userid, @name)";
                    using (var insertShotList = new SqlCommand(insertQuery, connection, transaction))
                    {
                        insertShotList.Parameters.AddWithValue("@shotid", shotid);
                        insertShotList.Parameters.AddWithValue("@userid", userid);
                        insertShotList.Parameters.AddWithValue("@name", shot.shotinfo.Name);

                        await insertShotList.ExecuteNonQueryAsync();
                    }

                    // Set up the SmartDot sensors with proper frequencies
                    long[] sensorIds = { 1, 2, 3, 4 };
                    long[] sensorsCreated = new long[4];
                    int i = 0;
                    foreach (var id in sensorIds)
                    {

                        insertQuery = @"INSERT INTO [SD_Sensor] (type_id, shotid)
                                    OUTPUT INSERTED.sensor_id
                                    VALUES (@typeid, @shotid)";
                        using (var insertSensor = new SqlCommand(insertQuery, connection, transaction))
                        {
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
                    }
                    // Batch query
                    var query = new StringBuilder(@"
                        INSERT INTO [SensorData] 
                        (sensor_id, count, xaxis, yaxis, zaxis, waxis, logtime) 
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
                        query.Append($"(@sensor_id{parameterIndex}, @count{parameterIndex}, ");
                        query.Append($"@xaxis{parameterIndex}, @yaxis{parameterIndex}, @zaxis{parameterIndex}, ");
                        query.Append($"@waxis{parameterIndex}, @logtime{parameterIndex}),");

                        // Add parameters for the row
                        parameters.Add(new SqlParameter($"@sensor_id{parameterIndex}", sensorid));
                        parameters.Add(new SqlParameter($"@count{parameterIndex}", data.Count ?? (object)DBNull.Value));


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
                    using (var command = new SqlCommand(query.ToString(), connection, transaction))
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                        await command.ExecuteNonQueryAsync();
                    }
                    // Commit the transaction
                    transaction.Commit();
                }
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

