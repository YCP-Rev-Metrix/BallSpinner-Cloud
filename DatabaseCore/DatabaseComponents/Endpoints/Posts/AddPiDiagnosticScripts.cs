using System.Reflection;
using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<int>> AddPiDiagnosticScripts(List<PiDiagnosticScript> scripts)
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
            foreach(PiDiagnosticScript obj in scripts)
            {
                PropertyInfo[] properties = scripts.GetType().GetProperties();
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
            
            string insertPiDiagnosticScriptQuery = @"
            INSERT INTO [Team_PI_Tables].[DiagnosticScript] (sessionId, time, motorId, instruction)
            OUTPUT INSERTED.id
            VALUES (@sessionId, @time, @motorId, @instruction)";
            
            using var piDiagnosticScriptCommand = new SqlCommand(insertPiDiagnosticScriptQuery, connection, transaction);
            List<int> insertedIds = new List<int>();
            
            foreach (PiDiagnosticScript obj in scripts)
            {
                piDiagnosticScriptCommand.Parameters.Clear();
                piDiagnosticScriptCommand.Parameters.AddWithValue("@sessionId", obj.SessionId);
                piDiagnosticScriptCommand.Parameters.AddWithValue("@time", obj.Time);
                piDiagnosticScriptCommand.Parameters.AddWithValue("@motorId", obj.MotorId);
                piDiagnosticScriptCommand.Parameters.AddWithValue("@instruction", obj.Instruction);

                object? result = await piDiagnosticScriptCommand.ExecuteScalarAsync();
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
            throw new Exception("Error adding PiDiagnosticScripts", e);
        }
    }
}