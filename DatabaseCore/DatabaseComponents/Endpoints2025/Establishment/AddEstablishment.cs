using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<int?> AddEstablishment(Establishment establishment)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new InvalidOperationException("Connection string is not set.");
        }

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        LogWriter.LogInfo(connection);

        const string insertQuery = @"
            INSERT INTO [combinedDB].[Establishments]
                (fullName, nickName, gpsLocation, homeHouse, reason, address, phoneNumber, lanes, type, location, enabled, MobileID)
            OUTPUT INSERTED.ID
            VALUES
                (@fullName, @nickName, @gpsLocation, @homeHouse, @reason, @address, @phoneNumber, @lanes, @type, @location, @enabled, @MobileID)";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@fullName", SqlDbType.VarChar, 100).Value = establishment.FullName ?? (object)DBNull.Value;
        command.Parameters.Add("@nickName", SqlDbType.VarChar, 100).Value = establishment.NickName ?? (object)DBNull.Value;
        command.Parameters.Add("@gpsLocation", SqlDbType.VarChar, 200).Value = establishment.GPSLocation ?? (object)DBNull.Value;
        command.Parameters.Add("@homeHouse", SqlDbType.Bit).Value = establishment.HomeHouse;
        command.Parameters.Add("@reason", SqlDbType.VarChar, 200).Value = establishment.Reason ?? (object)DBNull.Value;
        command.Parameters.Add("@address", SqlDbType.VarChar, 200).Value = establishment.Address ?? (object)DBNull.Value;
        command.Parameters.Add("@phoneNumber", SqlDbType.VarChar, 20).Value = establishment.PhoneNumber ?? (object)DBNull.Value;
        command.Parameters.Add("@lanes", SqlDbType.VarChar, 50).Value = establishment.Lanes ?? (object)DBNull.Value;
        command.Parameters.Add("@type", SqlDbType.VarChar, 50).Value = establishment.Type ?? (object)DBNull.Value;
        command.Parameters.Add("@location", SqlDbType.VarChar, 100).Value = establishment.Location ?? (object)DBNull.Value;
        command.Parameters.Add("@enabled", SqlDbType.Bit).Value = establishment.Enabled;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = establishment.MobileID.HasValue ? (object)establishment.MobileID.Value : DBNull.Value;

        object? result = await command.ExecuteScalarAsync();
        return result != null && result != DBNull.Value ? Convert.ToInt32(result) : null;
    }
}
