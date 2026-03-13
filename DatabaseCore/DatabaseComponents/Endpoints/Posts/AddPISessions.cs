using System.Reflection;
using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb 
{
    public async Task<List<int>> AddPISessions(List<PiSession> piSessions)
    {
        //ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        ConnectionString = Environment.GetEnvironmentVariable("LOCALDB_CONNECTION_STRING");
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
            foreach(PiSession obj in piSessions)
            {
                PropertyInfo[] properties = piSessions.GetType().GetProperties();
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
            
            string insertPISessionQuery = @"
            INSERT INTO [Team_PI_Tables].[PiSession] (name, timeStamp, isShotMode, spin_Instruction_points, tilt_Instruction_Points, angle_Instruction_Points) 
            OUTPUT INSERTED.id 
            VALUES (@name, @timeStamp, @isShotMode, @spin_Instruction_Points, @tilt_Instruction_Points, @angle_Instruction_Points)";

            using var piSessionCommand = new SqlCommand(insertPISessionQuery, connection, transaction);
            List<int> insertedIds = new List<int>();
            
            foreach (PiSession piSession in piSessions)
            {
                piSessionCommand.Parameters.Clear();
                piSessionCommand.Parameters.AddWithValue("@name", piSession.Name ?? string.Empty);
                piSessionCommand.Parameters.AddWithValue("@timeStamp", piSession.TimeStamp);
                piSessionCommand.Parameters.AddWithValue("@isShotMode", piSession.IsShotMode ?? false);
                piSessionCommand.Parameters.AddWithValue("@spin_Instruction_Points", piSession.Spin_Instruction_Points ?? string.Empty);
                piSessionCommand.Parameters.AddWithValue("@tilt_Instruction_Points", piSession.Tilt_Instruction_Points ?? string.Empty);
                piSessionCommand.Parameters.AddWithValue("@angle_Instruction_Points", piSession.Angle_Instruction_Points ?? string.Empty);

                object? result = await piSessionCommand.ExecuteScalarAsync();
                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException("Failed to insert PI Session or retrieve session ID.");
                }

                insertedIds.Add((int)result);
            }
            
            await transaction.CommitAsync();
            return insertedIds;
        } 
        catch (Exception ex)
        {
            throw new Exception("Error adding PI Sessions", ex);
        }
    }
}
