using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> Startpractice(int event_id)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        string insertQuery = "INSERT INTO [Practice] (event_id) " +
                                "VALUES (@Event_id)";
        using var command = new SqlCommand(insertQuery, connection);

        command.Parameters.Add("@Event_id", SqlDbType.BigInt).Value = event_id;

        int i = await command.ExecuteNonQueryAsync();
        return i != -1;

    }
}