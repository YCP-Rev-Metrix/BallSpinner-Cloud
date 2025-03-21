using Common.Logging;
using Common.POCOs;
using Common.POCOs.Shots;
using Microsoft.Data.SqlClient;
using System.Net.Mail;


namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<SimulatedShotList> GetShotsbyUsername(string? username)
    {
        var shots = new Dictionary<string?, SimulatedShot>();
        try
        {
            ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            // Get shot info and data points
            string selectQuery = @"
            SELECT ssl.name, iv.InitialPointx, iv.InitialPointy, iv.InflectionPointx, iv.InflectionPointy, iv.FinalPointx, iv.FinalPointy, iv.TimeStep, sds.sensor_id, sds.[type_id], sd.count, sd.xaxis, sd.yaxis, sd.zaxis, sd.waxis, sd.logtime
            FROM [User] AS u
            INNER JOIN SimulatedShotList AS ssl ON u.id = ssl.userid
            INNER JOIN SimulatedShot AS ss ON ssl.shotid = ss.shotid
            INNER JOIN SD_Sensor AS sds ON sds.shotid = ss.shotid
            INNER JOIN SensorData AS sd ON sds.sensor_id = sd.sensor_id
            INNER JOIN InitialValues as iv ON iv.id = ss.InitialValuesID
            WHERE u.username = @Username;";

            using var command = new SqlCommand(selectQuery, connection);

            command.Parameters.AddWithValue("@Username", username);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string? shotName = reader["name"].ToString();

                    if (!shots.ContainsKey(shotName))
                    {
                        Coordinate BezierInitPoint = new Coordinate(reader.GetFieldValue<double>(reader.GetOrdinal("InitialPointx")), reader.GetFieldValue<double>(reader.GetOrdinal("InitialPointy")));
                        Coordinate BezierInflectionPoint = new Coordinate(reader.GetFieldValue<double>(reader.GetOrdinal("InflectionPointx")), reader.GetFieldValue<double>(reader.GetOrdinal("InflectionPointy")));
                        Coordinate BezierFinalPoint = new Coordinate(reader.GetFieldValue<double>(reader.GetOrdinal("FinalPointx")), reader.GetFieldValue<double>(reader.GetOrdinal("FinalPointy")));
                        double TimeStep = reader.GetFieldValue<double>(reader.GetOrdinal("TimeStep"));
                        var simulatedShot = new ShotInfo
                        {
                            Name = shotName,
                            BezierInitPoint = BezierInitPoint,
                            BezierInflectionPoint = BezierInflectionPoint,
                            BezierFinalPoint = BezierFinalPoint,
                            TimeStep = TimeStep,
                        };
                        shots[shotName] = new SimulatedShot();
                        shots[shotName].shotinfo = simulatedShot;
                        shots[shotName].data = new List<SampleData?>();
                    }

                    var sampleData = new SampleData
                    {
                        Type = reader["type_id"].ToString(),
                        Count = reader.GetNullableValue<int>("count"),
                        X = reader.GetNullableValue<double>("xaxis"),
                        Y = reader.GetNullableValue<double>("yaxis"),
                        Z = reader.GetNullableValue<double>("zaxis"),
                        //W = reader.GetNullableValue<double>("waxis"),
                        Logtime = reader.GetNullableValue<double>("logtime")
                    };

                    shots[shotName].data.Add(sampleData);
                }
            }
            // Get ball and sensor info
            string infoQuery = @"
            SELECT b.name, b.weight, b.core_type, b.diameter, si.Comments, si.Date, sl.MACAddress
            FROM [User] as u
            INNER JOIN SimulatedShotList ssl ON u.id = ssl.userID
            INNER JOIN SimulatedShot ss ON ssl.shotid = ss.shotid
            INNER JOIN Ball b ON ss.ballID = b.ballID
            INNER JOIN SensorInfo si ON si.infoID = ss.smartdot_sensorsid
            INNER JOIN SensorList sl ON sl.SmartDotID = si.SmartDotID
            ";
            using var command2 = new SqlCommand(infoQuery, connection);
            using (var reader2 = await command2.ExecuteReaderAsync())
            {
                if (await reader2.ReadAsync())
                {
                    Ball ball = new Ball
                    {
                        Name = reader2.GetFieldValue<string>(reader2.GetOrdinal("name")),
                        Diameter = reader2.GetFieldValue<double>(reader2.GetOrdinal("diameter")),
                        CoreType = reader2.GetFieldValue<string>(reader2.GetOrdinal("core_type")),
                        Weight = reader2.GetFieldValue<double>(reader2.GetOrdinal("weight")),
                    };
                    SmartDotInfo info = new SmartDotInfo
                    {
                        MACAddress = reader2.GetFieldValue<byte[]>(reader2.GetOrdinal("MACAddress")),
                        Comments = reader2.GetFieldValue<string>(reader2.GetOrdinal("Comments")),
                        date = reader2.GetDateTime(reader2.GetOrdinal("Date")).ToString("yyyy-MM-dd"),
                    };
                }
                else
                {
                    throw new Exception("Unable to retrieve ball / sensor information");
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("Unable to retrieve ball / sensor information");
        }
        return new SimulatedShotList(shots.Values.ToList());
    }
    /* THESE METHODS NEED TO BE UPDATED DUE TO DRASTIC CHANGES MADE TO SHOT DATA (see above endpoint) BUT FOR NOW THEY ARE COMMENTED OUT BECAUSE THE APPLICATION DOES NOT USE THEM
    public async Task<SimulatedShotList> GetShotsByShotname(string? username, string? shotname)
    {
        var shots = new Dictionary<string?, SimulatedShot>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

       string selectQuery = @"
            SELECT ssl.name, ss.speed, ss.angle, ss.[position], sds.sensor_id, st.type, sds.frequency, sd.count, sd.xaxis, sd.yaxis, sd.zaxis, sd.waxis, sd.logtime
            FROM [User] AS u
            INNER JOIN SimulatedShotList AS ssl ON u.id = ssl.userid
            INNER JOIN SimulatedShot AS ss ON ssl.shotid = ss.shotid
            INNER JOIN SD_Sensor AS sds ON sds.shotid = ss.shotid
            INNER JOIN SensorData AS sd ON sds.sensor_id = sd.sensor_id
            INNER JOIN SensorType AS st ON st.type_id = sds.type_id
            WHERE u.username = @Username AND ssl.name = @Shotname";
                
        using var command = new SqlCommand(selectQuery, connection);
    
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@Shotname", shotname);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            string? shotName = reader["name"].ToString();
            if (!shots.ContainsKey(shotName))
            {
                var simulatedShot = new ShotInfo
                {
                    Name = shotName,
                    Angle = reader.GetNullableValue<double>("angle"),
                    Speed = reader.GetNullableValue<double>("speed"),
                    Position = reader.GetNullableValue<double>("position"),
                    Frequency = reader.GetNullableValue<double>("frequency")
                };
                shots[shotName] = new SimulatedShot();
                shots[shotName].shotinfo = simulatedShot;
                shots[shotName].data = new List<SampleData?>();
            }

            var sampleData = new SampleData
            {
                Type = reader["type"].ToString(),
                Count = reader.GetNullableValue<int>("count"),
                X = reader.GetNullableValue<double>("xaxis"),
                Y = reader.GetNullableValue<double>("yaxis"),
                Z = reader.GetNullableValue<double>("zaxis"),
                //W = reader.GetNullableValue<double>("waxis"),
                Logtime = reader.GetNullableValue<double>("logtime")
            };
            
            shots[shotName].data.Add(sampleData);
        }

        return new SimulatedShotList(shots.Values.ToList());
    }
    public async Task<SimulatedShotList> GetAllShots(string? username)
    {
        var shots = new Dictionary<string?, SimulatedShot>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        try
        {
            (bool success, string? roles) = await GetRoles(username);
            if (success && roles == "admin")
            {
                string selectQuery = @"
                SELECT ssl.name, ss.speed, ss.angle, ss.[position], sds.sensor_id, st.type, sds.frequency, sd.count, sd.xaxis, sd.yaxis, sd.zaxis, sd.waxis, sd.logtime
                FROM [User] AS u
                INNER JOIN SimulatedShotList AS ssl ON u.id = ssl.userid
                INNER JOIN SimulatedShot AS ss ON ssl.shotid = ss.shotid
                INNER JOIN SD_Sensor AS sds ON sds.shotid = ss.shotid
                INNER JOIN SensorType AS st ON st.type_id = sds.type_id
                INNER JOIN SensorData AS sd ON sds.sensor_id = sd.sensor_id";

                using var command = new SqlCommand(selectQuery, connection);
                
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    string? shotName = reader["name"].ToString();

                    if (!shots.ContainsKey(shotName))
                    {
                        var simulatedShot = new ShotInfo
                        { 
                            Name = shotName,
                            Angle = reader.GetNullableValue<double>("angle"),
                            Speed = reader.GetNullableValue<double>("speed"),
                            Position = reader.GetNullableValue<double>("position"),
                            Frequency = reader.GetNullableValue<double>("frequency")
                        };
                        shots[shotName] = new SimulatedShot();
                        shots[shotName].shotinfo = simulatedShot;
                        shots[shotName].data = new List<SampleData?>();
                    }

                    var sampleData = new SampleData
                    {
                        Type = reader["type"].ToString(),
                        Count = reader.GetNullableValue<int>("count"),
                        X = reader.GetNullableValue<double>("xaxis"),
                        Y = reader.GetNullableValue<double>("yaxis"),
                        Z = reader.GetNullableValue<double>("zaxis"),
                        //W = reader.GetNullableValue<double>("waxis"),
                        Logtime = reader.GetNullableValue<double>("logtime")
                    };
                    shots[shotName].data.Add(sampleData);
                } 
                return new SimulatedShotList(shots.Values.ToList());
            }
            throw new Exception($"User: {username} is not an administrator");
        }
        catch (Exception e)
        {
            LogWriter.LogInfo(e.StackTrace);
            throw;
        }
    }
    */
}

