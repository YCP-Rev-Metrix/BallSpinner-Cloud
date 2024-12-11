using System.Data;
using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddSmartDot(SmartDot smartDot, string username)
{
    // Look into user table, get id that matches with username
    using var connection = new SqlConnection(ConnectionString);
    await connection.OpenAsync();

    using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();
    try
    {
        // Get User Id
        int userId = await GetUserId(username);
        if (userId <= 0)
        {
            LogWriter.LogError($"Invalid user ID for username: {username}");
            return false;
        }

        // Insert into SD table and get the generated ID
        string insertSmartDotQuery = @"
            INSERT INTO [SmartDot] (name, address) 
            OUTPUT INSERTED.smartdot_id 
            VALUES (@name, @address)";

        using var ballCommand = new SqlCommand(insertSmartDotQuery, connection, transaction);
        ballCommand.Parameters.AddWithValue("@name", smartDot.Name);
        ballCommand.Parameters.AddWithValue("@address", smartDot.MacAddress);

        object? result = await ballCommand.ExecuteScalarAsync();
        if (result == null || result == DBNull.Value)
        {
            throw new InvalidOperationException("Failed to insert smartdot or retrieve smartdot ID.");
        }

        int smartdotId = Convert.ToInt32(result);

        // Insert into Arsenal table
        string insertArsenalQuery = @"
            INSERT INTO [SmartDotList] (userid, smartdot_id) 
            VALUES (@userid, @smartdot_id)";

        using var arsenalCommand = new SqlCommand(insertArsenalQuery, connection, transaction);
        arsenalCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int) { Value = userId });
        arsenalCommand.Parameters.Add(new SqlParameter("@smartdot_id", SqlDbType.Int) { Value = smartdotId });

        int rowsAffected = await arsenalCommand.ExecuteNonQueryAsync();
        if (rowsAffected <= 0)
        {
            throw new InvalidOperationException("Failed to associate smartdot with user in smartdotlist.");
        }

        // Commit the transaction
        await transaction.CommitAsync();
        return true;
    }
    catch (Exception ex)
    {
        // Log exception
        LogWriter.LogError($"Error occurred while adding a smartdot for user '{username}': {ex.Message}\n\n"+ ex);

        // Rollback the transaction in case of any failure
        if (transaction != null)
        {
            await transaction.RollbackAsync();
        }

        throw; // Re-throw the exception after rollback
    }
}
}