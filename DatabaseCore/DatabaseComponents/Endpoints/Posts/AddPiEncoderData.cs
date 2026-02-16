using System.Reflection;
using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<int>> AddPiEncoderData(List<PiEncoderData> data)
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
            // Check for null properties in each PiEncoderData object
            // Should not have any null properties, return -1 if any found
            foreach(PiEncoderData obj in data)
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
            INSERT INTO [Team_PI_Tables].[EncoderData] (sessionId, time, pulses, motorId, replayIteration)
            OUTPUT INSERTED.id
            VALUES (@sessionId, @time, @pulses, @motorId, @replayIteration);";
            
            using var piEncoderDataCommand = new SqlCommand(insertQuery, connection, transaction);
            List<int> insertedIds = new List<int>();
            foreach (PiEncoderData obj in data)
            {
                piEncoderDataCommand.Parameters.Clear();
                piEncoderDataCommand.Parameters.AddWithValue("@sessionId", obj.SessionId);
                piEncoderDataCommand.Parameters.AddWithValue("@time", obj.Time);
                piEncoderDataCommand.Parameters.AddWithValue("@pulses", obj.Pulses);
                piEncoderDataCommand.Parameters.AddWithValue("@motorId", obj.MotorId);
                piEncoderDataCommand.Parameters.AddWithValue("@replayIteration", obj.ReplayIteration);
                
                object? result = await piEncoderDataCommand.ExecuteScalarAsync();
                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException("Failed to insert Pi Encoder Data or retrieve session ID.");
                }
                
                insertedIds.Add((int)result);
            }
            
            await transaction.CommitAsync();
            return insertedIds;
        }
        catch (Exception e)
        {
            throw new Exception("Error adding PiEncoderData", e);
        }
    }
}