using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddShot(Shot shot)
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

        string insertQuery = "INSERT INTO [combinedDB].[Shots] (Type, SmartDotID, SessionID, BallID, FrameID, ShotNumber, LeaveType, Side, Position, Comment, MobileID) " +
                             "VALUES (@Type, @SmartDotID, @SessionID, @BallID, @FrameID, @ShotNumber, @LeaveType, @Side, @Position, @Comment, @MobileID)";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@Type", SqlDbType.Int).Value = shot.Type ?? (object)DBNull.Value;
        command.Parameters.Add("@SmartDotID", SqlDbType.Int).Value = shot.SmartDotId ?? (object)DBNull.Value;
        command.Parameters.Add("@SessionID", SqlDbType.Int).Value = shot.SessionId ?? (object)DBNull.Value;
        command.Parameters.Add("@BallID", SqlDbType.Int).Value = shot.BallId ?? (object)DBNull.Value;
        command.Parameters.Add("@FrameID", SqlDbType.Int).Value = shot.FrameId ?? (object)DBNull.Value;
        command.Parameters.Add("@ShotNumber", SqlDbType.Int).Value = shot.ShotNumber ?? (object)DBNull.Value;
        command.Parameters.Add("@LeaveType", SqlDbType.Int).Value = shot.LeaveType ?? (object)DBNull.Value;
        command.Parameters.Add("@Side", SqlDbType.VarChar).Value = shot.Side ?? string.Empty;
        command.Parameters.Add("@Position", SqlDbType.VarChar).Value = shot.Position ?? string.Empty;
        command.Parameters.Add("@Comment", SqlDbType.VarChar).Value = shot.Comment ?? string.Empty;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = shot.MobileID.HasValue ? (object)shot.MobileID.Value : DBNull.Value;

        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
}
