using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddShotForUser(Shot shot, string? username, int? mobileID = null)
    {
        if (shot == null) return false;
        if (!shot.SessionId.HasValue || shot.SessionId.Value <= 0) return false;

        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return false;

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Ensure the referenced Session belongs to the authenticated user (via Events ownership)
        const string ownsSessionQuery = @"
            SELECT COUNT(1)
            FROM [combinedDB].[Sessions] s
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE s.ID = @SessionId AND e.UserId = @UserId;";
        using (var ownsSessionCmd = new SqlCommand(ownsSessionQuery, connection))
        {
            ownsSessionCmd.Parameters.Add("@SessionId", SqlDbType.Int).Value = shot.SessionId.Value;
            ownsSessionCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            int owns = Convert.ToInt32(await ownsSessionCmd.ExecuteScalarAsync());
            if (owns <= 0) return false;
        }

        const string insertQuery = @"
            INSERT INTO [combinedDB].[Shots]
                (Type, SmartDotID, SessionID, BallID, FrameID, ShotNumber, LeaveType, Side, Position, Comment, MobileID)
            VALUES
                (@Type, @SmartDotID, @SessionID, @BallID, @FrameID, @ShotNumber, @LeaveType, @Side, @Position, @Comment, @MobileID);";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@Type", SqlDbType.Int).Value = shot.Type ?? (object)DBNull.Value;
        command.Parameters.Add("@SmartDotID", SqlDbType.Int).Value = shot.SmartDotId ?? (object)DBNull.Value;
        command.Parameters.Add("@SessionID", SqlDbType.Int).Value = shot.SessionId.Value;
        command.Parameters.Add("@BallID", SqlDbType.Int).Value = shot.BallId ?? (object)DBNull.Value;
        command.Parameters.Add("@FrameID", SqlDbType.Int).Value = shot.FrameId ?? (object)DBNull.Value;
        command.Parameters.Add("@ShotNumber", SqlDbType.Int).Value = shot.ShotNumber ?? (object)DBNull.Value;
        command.Parameters.Add("@LeaveType", SqlDbType.Int).Value = shot.LeaveType ?? (object)DBNull.Value;
        command.Parameters.Add("@Side", SqlDbType.VarChar).Value = shot.Side ?? string.Empty;
        command.Parameters.Add("@Position", SqlDbType.VarChar).Value = shot.Position ?? string.Empty;
        command.Parameters.Add("@Comment", SqlDbType.VarChar).Value = shot.Comment ?? string.Empty;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = shot.MobileID.HasValue ? (object)shot.MobileID.Value : DBNull.Value;

        return await command.ExecuteNonQueryAsync() > 0;
    }
}

