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

        const string selectQuery = "SELECT ID, Name, Lanes, Type, Location FROM [combinedDB].[Establishments]";

        using var command = new SqlCommand(selectQuery, connection);

        var establishments = new List<Establishment>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var establishment = new Establishment
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                Name = reader["Name"] as string,
                Lanes = reader["Lanes"] as string,
                Type = reader["Type"] as string,
                Location = reader["Location"] as string
            };

            establishments.Add(establishment);
        }

        return (establishments.Any(), establishments);
    }
}