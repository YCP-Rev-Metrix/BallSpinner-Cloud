using System.Reflection;
using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<int>> AddPiHeatData(List<PiHeatData> data)
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
            // Check for null properties in each PiHeatData object
            // Should not have any null properties, return -1 if any found
            foreach(PiHeatData obj in data)
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
            INSERT INTO [Team_PI_Tables].[HeatData]
            (sessionId, time, value, motorId, replayIteration)
            OUTPUT INSERTED.id
            VALUES (@sessionId, @time, @value, @motorId, @replayIteration);";
            
            using var piHeatDataCommand = new SqlCommand(insertQuery, connection, transaction);
            List<int> insertedIds = new List<int>();
            foreach (PiHeatData obj in data)
            {
                piHeatDataCommand.Parameters.Clear();
                piHeatDataCommand.Parameters.AddWithValue("@sessionId", obj.SessionId);
                piHeatDataCommand.Parameters.AddWithValue("@time", obj.Time);
                piHeatDataCommand.Parameters.AddWithValue("@value", obj.Value);
                piHeatDataCommand.Parameters.AddWithValue("@motorId", obj.MotorId);
                piHeatDataCommand.Parameters.AddWithValue("@replayIteration", obj.ReplayIteration);
                
                object? result = await piHeatDataCommand.ExecuteScalarAsync();
                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException("Failed to insert Pi Heat Data or retrieve session ID.");
                }
                
                insertedIds.Add((int)result);
            }

            await transaction.CommitAsync();
            return insertedIds;
        }
        catch (Exception e)
        {
            throw new Exception("Error adding PiHeatData", e);
        }
    }
}