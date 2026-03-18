using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddEstablishment(Establishment establishment)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new InvalidOperationException("Connection string is not set.");
        }

        using var connection = new SqlConnection(ConnectionString);
        try
        {
            await connection.OpenAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
        LogWriter.LogInfo(connection);

        const string insertQuery = "INSERT INTO [combinedDB].[Establishments] (Name, Lanes, Type, Location, MobileID) " +
                             "VALUES (@Name, @Lanes, @Type, @Location, @MobileID)";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = establishment.Name ?? (object)DBNull.Value;
        command.Parameters.Add("@Lanes", SqlDbType.VarChar, 50).Value = establishment.Lanes ?? (object)DBNull.Value;
        command.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = establishment.Type ?? (object)DBNull.Value;
        command.Parameters.Add("@Location", SqlDbType.VarChar, 50).Value = establishment.Location ?? (object)DBNull.Value;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = establishment.MobileID.HasValue ? (object)establishment.MobileID.Value : DBNull.Value;

        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
}
