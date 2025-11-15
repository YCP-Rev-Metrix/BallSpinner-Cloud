using System.Reflection;
using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb 
{
    public async Task<List<int>> AddPISessions(List<PiSession> piSessions)
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
            foreach(PiSession obj in piSessions)
            {
                PropertyInfo[] properties = piSessions.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach(PropertyInfo property in properties)
                {
                    object value = property.GetValue(obj);

                    if (value == null)
                    { 
                        List<int> error = new List<int> { -1 };
                        return error;
                    }
                }
            }
            
            string insertPISessionQuery = @"
            INSERT INTO [Team_PI_Tables].[PI_Sessions] (name, timeStamp, isShotMode) 
            OUTPUT INSERTED.id 
            VALUES (@name, @timeStamp, @isShotMode)";

            using var piSessionCommand = new SqlCommand(insertPISessionQuery, connection, transaction);
            List<int> insertedIds = new List<int>();
            
            foreach (PiSession piSession in piSessions)
            {
                piSessionCommand.Parameters.Clear();
                piSessionCommand.Parameters.AddWithValue("@name", piSession.Name ?? string.Empty);
                piSessionCommand.Parameters.AddWithValue("@timeStamp", piSession.TimeStamp);
                piSessionCommand.Parameters.AddWithValue("@isShotMode", piSession.IsShotMode ?? false);

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
