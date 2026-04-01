using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<Establishment> establishments)> GetAppEstablishments()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        const string selectQuery = @"
            SELECT ID, fullName, nickName, gpsLocation, homeHouse, reason, address, phoneNumber,
                   lanes, type, location, enabled, MobileID
            FROM [combinedDB].[Establishments]";

        using var command = new SqlCommand(selectQuery, connection);

        var establishments = new List<Establishment>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            establishments.Add(new Establishment
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                MobileID = reader["MobileID"] != DBNull.Value ? Convert.ToInt32(reader["MobileID"]) : null,
                FullName = reader["fullName"] as string,
                NickName = reader["nickName"] as string,
                GPSLocation = reader["gpsLocation"] as string,
                HomeHouse = reader["homeHouse"] != DBNull.Value && Convert.ToBoolean(reader["homeHouse"]),
                Reason = reader["reason"] as string,
                Address = reader["address"] as string,
                PhoneNumber = reader["phoneNumber"] as string,
                Lanes = reader["lanes"] as string,
                Type = reader["type"] as string,
                Location = reader["location"] as string,
                Enabled = reader["enabled"] != DBNull.Value && Convert.ToBoolean(reader["enabled"])
            });
        }

        return (establishments.Any(), establishments);
    }
}
