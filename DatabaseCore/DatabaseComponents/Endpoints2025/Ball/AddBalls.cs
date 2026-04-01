using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddBalls(Common.POCOs.MobileApp.Ball ball, string username, int? mobileID = null)
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new InvalidOperationException("Connection string is not set.");
        }

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            int userId = mobileID.HasValue && mobileID.Value > 0
                ? await GetUserId(username, mobileID)
                : await GetUserId(username);
            if (userId <= 0)
            {
                LogWriter.LogError($"Invalid user ID for username: {username}");
                return false;
            }

            string insertBallQuery = @"
            INSERT INTO [combinedDB].[Balls] (userId, name, ballMFG, ballMFGName, serialNumber, weight, core, colorString, coverstock, comment, enabled, mobileId)
            OUTPUT INSERTED.id
            VALUES (@userId, @name, @ballMFG, @ballMFGName, @serialNumber, @weight, @core, @colorString, @coverstock, @comment, @enabled, @mobileId)";

            using var ballCommand = new SqlCommand(insertBallQuery, connection, transaction);
            ballCommand.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            ballCommand.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = ball.Name ?? string.Empty;
            ballCommand.Parameters.Add("@ballMFG", SqlDbType.VarChar, 100).Value = ball.BallMFG ?? (object)DBNull.Value;
            ballCommand.Parameters.Add("@ballMFGName", SqlDbType.VarChar, 100).Value = ball.BallMFGName ?? (object)DBNull.Value;
            ballCommand.Parameters.Add("@serialNumber", SqlDbType.VarChar, 100).Value = ball.SerialNumber ?? (object)DBNull.Value;
            ballCommand.Parameters.Add("@weight", SqlDbType.Int).Value = ball.Weight.HasValue ? (object)ball.Weight.Value : DBNull.Value;
            ballCommand.Parameters.Add("@core", SqlDbType.VarChar, 100).Value = ball.Core ?? (object)DBNull.Value;
            ballCommand.Parameters.Add("@colorString", SqlDbType.VarChar, 50).Value = ball.ColorString ?? (object)DBNull.Value;
            ballCommand.Parameters.Add("@coverstock", SqlDbType.VarChar, 100).Value = ball.Coverstock ?? (object)DBNull.Value;
            ballCommand.Parameters.Add("@comment", SqlDbType.VarChar, 500).Value = ball.Comment ?? (object)DBNull.Value;
            ballCommand.Parameters.Add("@enabled", SqlDbType.Bit).Value = ball.Enabled;
            ballCommand.Parameters.Add("@mobileId", SqlDbType.Int).Value = ball.MobileID.HasValue ? (object)ball.MobileID.Value : DBNull.Value;

            object? result = await ballCommand.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert ball or retrieve ball ID.");
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            LogWriter.LogError($"Error occurred while adding a ball for user '{username}': {ex.Message}\n{ex}");
            await transaction.RollbackAsync();
            throw;
        }
    }
}
