using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using Common.POCOs;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<int> GetUserId(string? username)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT id FROM [User] WHERE username = @Username"; // Adjusted to select more fields
        
        using var command = new SqlCommand(selectQuery, connection);
        
        command.Parameters.AddWithValue("@Username", username);
            
        var result = command.ExecuteScalarAsync();
        return result.Result == DBNull.Value ? 0 : Convert.ToInt32(result.Result);
    }

    // Overload used by delete endpoints: match the user row by both username and mobileId when possible.
    public async Task<int> GetUserId(string? username, int? mobileID)
    {
        // Fallback to username-only if mobileID isn't provided.
        if (!mobileID.HasValue || mobileID.Value <= 0)
            return await GetUserId(username);

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Other tables in this codebase use `mobileId` (lower camel) column naming.
        try
        {
            string selectQuery = "SELECT id FROM [User] WHERE username = @Username AND mobileId = @MobileID";

            using var command = new SqlCommand(selectQuery, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@MobileID", mobileID.Value);

            var result = await command.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                // If mobileId didn't match (e.g. older rows / not populated), fall back to username-only
                // so replace flows don't hard-fail.
                return await GetUserId(username);
            }

            return Convert.ToInt32(result);
        }
        catch (SqlException)
        {
            // If the DB hasn't been updated with the mobileId column yet,
            // keep deletes working via username-only.
            return await GetUserId(username);
        }
    }
}