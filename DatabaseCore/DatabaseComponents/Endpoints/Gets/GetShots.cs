
/*
 * SELECT ss.name, ss.speed , ss.angle, ss.[position] , sds.[type] , sds.frequency, sd.sample_number , sd.xaxis , sd.yaxis , sd.zaxis , sd.waxis , sd.logtime 
   FROM [revmetrix-test].dbo.[User] as u
   INNER JOIN [revmetrix-test].dbo.SimulatedShotList as ssl ON u.id = ssl.userid 
   INNER JOIN [revmetrix-test].dbo.SimulatedShot as ss ON ssl.shotid = ss.shotid
   INNER JOIN [revmetrix-test].dbo.SD_Sensor as sds ON sds.shotid = ss.shotid
   INNER JOIN [revmetrix-test].dbo.SensorData sd ON  sds.sensor_id = sd.sensor_id 
   WHERE u.username = 'clualua' AND ss.name = 'shots'
 */

using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;


namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<ShotList> GetShotsbyUsername(string? username)
    {
        var shots = new Dictionary<string?, Shot>();
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = @"
            SELECT ss.name, ss.speed, ss.angle, ss.[position], sds.sensor_id, sds.[type], sds.frequency, sd.count, sd.xaxis, sd.yaxis, sd.zaxis, sd.waxis, sd.logtime
            FROM [User] AS u
            INNER JOIN SimulatedShotList AS ssl ON u.id = ssl.userid
            INNER JOIN SimulatedShot AS ss ON ssl.shotid = ss.shotid
            INNER JOIN SD_Sensor AS sds ON sds.shotid = ss.shotid
            INNER JOIN SensorData AS sd ON sds.sensor_id = sd.sensor_id
            WHERE u.username = @Username;";

        using var command = new SqlCommand(selectQuery, connection);

        command.Parameters.AddWithValue("@Username", username);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            string? shotName = reader["name"].ToString();

            if (!shots.ContainsKey(shotName))
            {
                var simulatedShot = new SimulatedShot
                {
                    Name = shotName,
                    Angle = reader.GetNullableValue<double>("angle"),
                    Speed = reader.GetNullableValue<double>("speed"),
                    Position = reader.GetNullableValue<double>("position"),
                    Frequency = reader.GetNullableValue<double>("frequency")
                };
                shots[shotName] = new Shot(simulatedShot);
            }

            var sampleData = new SampleData
            {
                Type = reader["type"].ToString(),
                Count = reader.GetNullableValue<int>("count"),
                X = reader.GetNullableValue<double>("xaxis"),
                Y = reader.GetNullableValue<double>("yaxis"),
                Z = reader.GetNullableValue<double>("zaxis"),
                W = reader.GetNullableValue<double>("waxis"),
                Logtime = reader.GetNullableValue<double>("logtime")
            };

            shots[shotName].Data.Add(sampleData);
        }
        return new ShotList(shots.Values.ToList());
    }

    public async Task<ShotList> GetShotsByShotname(string? username, string? shotname)
    {
        var shots = new Dictionary<string?, Shot>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

       string selectQuery = @"
            SELECT ss.name, ss.speed, ss.angle, ss.[position], sds.sensor_id, sds.[type], sds.frequency, sd.count, sd.xaxis, sd.yaxis, sd.zaxis, sd.waxis, sd.logtime
            FROM [User] AS u
            INNER JOIN SimulatedShotList AS ssl ON u.id = ssl.userid
            INNER JOIN SimulatedShot AS ss ON ssl.shotid = ss.shotid
            INNER JOIN SD_Sensor AS sds ON sds.shotid = ss.shotid
            INNER JOIN SensorData AS sd ON sds.sensor_id = sd.sensor_id
            WHERE u.username = @Username AND ss.name = @Shotname";
                
        using var command = new SqlCommand(selectQuery, connection);
    
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@Shotname", shotname);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            string? shotName = reader["name"].ToString();
            if (!shots.ContainsKey(shotName))
            {
                var simulatedShot = new SimulatedShot
                {
                    Name = shotName,
                    Angle = reader.GetNullableValue<double>("angle"),
                    Speed = reader.GetNullableValue<double>("speed"),
                    Position = reader.GetNullableValue<double>("position"),
                    Frequency = reader.GetNullableValue<double>("frequency")
                };
                shots[shotName] = new Shot(simulatedShot);
            }

            var sampleData = new SampleData
            {
                Type = reader["type"].ToString(),
                Count = reader.GetNullableValue<int>("count"),
                X = reader.GetNullableValue<double>("xaxis"),
                Y = reader.GetNullableValue<double>("yaxis"),
                Z = reader.GetNullableValue<double>("zaxis"),
                W = reader.GetNullableValue<double>("waxis"),
                Logtime = reader.GetNullableValue<double>("logtime")
            };
            
            shots[shotName].Data.Add(sampleData);
        }

        return new ShotList(shots.Values.ToList());
    }
    public async Task<ShotList> GetAllShots(string? username)
    {
        var shots = new Dictionary<string?, Shot>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        try
        {
            (bool success, string? roles) = await GetRoles(username);
            if (success && roles == "admin")
            {
                string selectQuery = @"
                SELECT ss.name, ss.speed, ss.angle, ss.[position], sds.sensor_id, sds.[type], sds.frequency, sd.count, sd.xaxis, sd.yaxis, sd.zaxis, sd.waxis, sd.logtime
                FROM [User] AS u
                INNER JOIN SimulatedShotList AS ssl ON u.id = ssl.userid
                INNER JOIN SimulatedShot AS ss ON ssl.shotid = ss.shotid
                INNER JOIN SD_Sensor AS sds ON sds.shotid = ss.shotid
                INNER JOIN SensorData AS sd ON sds.sensor_id = sd.sensor_id";

                using var command = new SqlCommand(selectQuery, connection);
                
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    string? shotName = reader["name"].ToString();

                    if (!shots.ContainsKey(shotName))
                    {
                        var simulatedShot = new SimulatedShot
                        { 
                            Name = shotName,
                            Angle = reader.GetNullableValue<double>("angle"),
                            Speed = reader.GetNullableValue<double>("speed"),
                            Position = reader.GetNullableValue<double>("position"),
                            Frequency = reader.GetNullableValue<double>("frequency")
                        };
                        shots[shotName] = new Shot(simulatedShot);
                    }

                    var sampleData = new SampleData
                    {
                        Type = reader["type"].ToString(),
                        Count = reader.GetNullableValue<int>("count"),
                        X = reader.GetNullableValue<double>("xaxis"),
                        Y = reader.GetNullableValue<double>("yaxis"),
                        Z = reader.GetNullableValue<double>("zaxis"),
                        W = reader.GetNullableValue<double>("waxis"),
                        Logtime = reader.GetNullableValue<double>("logtime")
                    };
                    shots[shotName].Data.Add(sampleData);
                } 
                return new ShotList(shots.Values.ToList());
            }
            throw new Exception($"User {username} is not an administrator");
        }
        catch (Exception e)
        {
            LogWriter.LogInfo(e.StackTrace);
            throw;
        }
    }
}


public static class SqlDataReaderExtensions
{
    public static T? GetNullableValue<T>(this SqlDataReader reader, string columnName) where T : struct
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? (T?)null : reader.GetFieldValue<T>(ordinal);
    }
}