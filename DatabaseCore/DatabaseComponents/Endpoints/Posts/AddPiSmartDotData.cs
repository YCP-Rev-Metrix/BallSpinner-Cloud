using System.Reflection;
using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<int>> AddPiSmartDotData(List<PiSmartDotData> data)
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
            // Check for null properties in each PiSession object
            // Should not have any null properties, return -1 if any found
            foreach(PiSmartDotData obj in data)
            {
                PropertyInfo[] properties = data.GetType().GetProperties();
                foreach(PropertyInfo property in properties)
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
            
            string insertQuery = @"
            INSERT INTO [Team_PI_Tables].[SmartDotData]
            (sessionId, time, accelX, accelY, accelZ, gyroX, gyroY, gyroZ, magnoX, magnoY, magnoZ, light)
            OUTPUT INSERTED.id
            VALUES (@sessionId, @time, @accelX, @accelY, @accelZ, @gyroX, @gyroY, @gyroZ, @magnoX, @magnoY, @magnoZ, @light);";
            
            using var piSmartDotDataCommand = new SqlCommand(insertQuery, connection, transaction);
            List<int> insertedIds = new List<int>();
            foreach (PiSmartDotData obj in data)
            {
                piSmartDotDataCommand.Parameters.Clear();
                piSmartDotDataCommand.Parameters.AddWithValue("@sessionId", obj.SessionId);
                piSmartDotDataCommand.Parameters.AddWithValue("@time", obj.Time);
                piSmartDotDataCommand.Parameters.AddWithValue("@accelX", obj.XL_X);
                piSmartDotDataCommand.Parameters.AddWithValue("@accelY", obj.XL_Y);
                piSmartDotDataCommand.Parameters.AddWithValue("@accelZ", obj.XL_Z);
                piSmartDotDataCommand.Parameters.AddWithValue("@gyroX", obj.GY_X);
                piSmartDotDataCommand.Parameters.AddWithValue("@gyroY", obj.GY_Y);
                piSmartDotDataCommand.Parameters.AddWithValue("@gyroZ", obj.GY_Z);
                piSmartDotDataCommand.Parameters.AddWithValue("@magnoX", obj.MG_X);
                piSmartDotDataCommand.Parameters.AddWithValue("@magnoY", obj.MG_Y);
                piSmartDotDataCommand.Parameters.AddWithValue("@magnoZ", obj.MG_Z);
                piSmartDotDataCommand.Parameters.AddWithValue("@light", obj.LT);

                object? result = await piSmartDotDataCommand.ExecuteScalarAsync();
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
            throw new Exception("Error adding Pi Smart Dot Data: " + e.Message);
        }
    }
}