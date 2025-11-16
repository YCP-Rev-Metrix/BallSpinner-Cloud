using System.Reflection;
using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<int>> AddPiShots(List<PiShot> shots)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new InvalidOperationException("Connection string is not set.");
        }

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            foreach (PiShot obj in shots)
            {
                PropertyInfo[] properties = shots.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        string value = (string)property.GetValue(obj);

                        if (string.IsNullOrEmpty(value))
                        {
                            List<int> error = new List<int> { -1 };
                            return error;
                        }
                    }
                }
            }

            string insertPiShotQuery = @"
            INSERT INTO [Team_PI_Tables].[ShotScript] (sessionId, time, rpm, angleDegrees, tiltDegrees)
            OUTPUT INSERTED.id
            VALUES (@sessionId, @time, @rpm, @angleDegrees, @tilt)";
            
            using var piShotCommand = new SqlCommand(insertPiShotQuery, connection, transaction);
            List<int> insertedIds = new List<int>();

            foreach (PiShot obj in shots)
            {
                piShotCommand.Parameters.Clear();
                piShotCommand.Parameters.AddWithValue("@sessionId", obj.SessionId);
                piShotCommand.Parameters.AddWithValue("@time", obj.Time);
                piShotCommand.Parameters.AddWithValue("@rpm", obj.Rpm);
                piShotCommand.Parameters.AddWithValue("@angleDegrees", obj.AngleDegrees);
                piShotCommand.Parameters.AddWithValue("@tilt", obj.TiltDegrees);

                object? result = await piShotCommand.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    insertedIds.Add(insertedId);
                }
            }

            await transaction.CommitAsync();
            return insertedIds;
        }
        catch (Exception e)
        {
            throw new Exception("Error adding PiShots", e);
        }
    }
}