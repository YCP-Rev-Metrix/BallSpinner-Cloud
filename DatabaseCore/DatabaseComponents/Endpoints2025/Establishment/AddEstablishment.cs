using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddEstablishment(string? name, string? lanes, string? type, string? location)
    {
        // If not local use Server conn string, if local use local conn string
        //ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection1 = new SqlConnection(ConnectionString);
        try
        {
            await connection1.OpenAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
        LogWriter.LogInfo(connection1);

        string insertQuery = "INSERT INTO [combinedDB].[Establishments] (Name, Lanes, Type, Location) " +
                             "VALUES (@Name, @Lanes, @Type, @Location)";

        using var command = new SqlCommand(insertQuery, connection1);

        command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;
        command.Parameters.Add("@Lanes", SqlDbType.VarChar, 50).Value = lanes;
        command.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = type;
        command.Parameters.Add("@Location", SqlDbType.VarChar, 50).Value = location;

        // Execute the query
        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
}