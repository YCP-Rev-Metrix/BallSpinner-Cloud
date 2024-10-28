using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> Startevent(int user_id, string event_type)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string insertQuery = "INSERT INTO [Event] (userid , event_type) " +
                                "VALUES (@User_id, @Event_type)";
        using var command = new SqlCommand(insertQuery, connection);

        command.Parameters.Add("@User_id", SqlDbType.BigInt).Value = user_id;
        command.Parameters.Add("@Event_type", SqlDbType.VarChar).Value = event_type;


        int i = await command.ExecuteNonQueryAsync();
        return i != -1;

    }
}