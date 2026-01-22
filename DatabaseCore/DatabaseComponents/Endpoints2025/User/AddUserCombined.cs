using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddUserCombined(string? firstname, string? lastname, string? username, byte[] hashedPassword, string? phone, string? email, string lastLogin, string hand)
    {
        // If not local use Server conn string, if local use local conn string
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        //ConnectionString = Environment.GetEnvironmentVariable("LOCALDB_CONNECTION_STRING");
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

        string insertQuery = "INSERT INTO combinedDB.[Users] (Username, Firstname, Lastname, HashedPassword, Email, PhoneNumber, LastLogin, Hand) " +
                             "VALUES (@Username, @Firstname, @Lastname, @HashedPassword, @Email, @PhoneNumber, @LastLogin, @Hand)";

        using var command = new SqlCommand(insertQuery, connection1);
        if (username is null)
        {
            await connection1.CloseAsync();
            return false;
        }

        command.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
        command.Parameters.Add("@Firstname", SqlDbType.VarChar, 50).Value = (object?)firstname ?? DBNull.Value;
        command.Parameters.Add("@Lastname", SqlDbType.VarChar, 50).Value = (object?)lastname ?? DBNull.Value;
        command.Parameters.Add("@HashedPassword", SqlDbType.VarBinary, -1).Value = (object?)hashedPassword ?? DBNull.Value;
        command.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = (object?)email ?? DBNull.Value;
        command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 15).Value = (object?)phone ?? DBNull.Value;
        command.Parameters.Add("@LastLogin", SqlDbType.VarChar).Value = (object?)lastLogin ?? DBNull.Value;
        command.Parameters.Add("@Hand", SqlDbType.VarChar).Value = (object?)hand ?? DBNull.Value;


        // Execute the query
        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
}