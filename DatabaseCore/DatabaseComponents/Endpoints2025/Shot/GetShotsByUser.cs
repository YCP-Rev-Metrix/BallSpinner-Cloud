using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Shot>> GetShotsByUser(string? username, int? mobileID = null)
    {
        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return new List<Shot>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        const string selectQuery = @"
            SELECT sh.ID, sh.Type, sh.SmartDotID, sh.SessionID, sh.BallID, sh.FrameID, sh.ShotNumber,
                   sh.LeaveType, sh.Side, sh.Position, sh.Comment, sh.MobileID
            FROM [combinedDB].[Shots] sh
            INNER JOIN [combinedDB].[Sessions] s ON s.ID = sh.SessionID
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE e.UserId = @UserId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        var shots = new List<Shot>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            shots.Add(new Shot
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                MobileID = reader["MobileID"] != DBNull.Value ? Convert.ToInt32(reader["MobileID"]) : null,
                Type = reader["Type"] != DBNull.Value ? Convert.ToInt32(reader["Type"]) : null,
                SmartDotId = reader["SmartDotID"] != DBNull.Value ? Convert.ToInt32(reader["SmartDotID"]) : null,
                SessionId = reader["SessionID"] != DBNull.Value ? Convert.ToInt32(reader["SessionID"]) : null,
                BallId = reader["BallID"] != DBNull.Value ? Convert.ToInt32(reader["BallID"]) : null,
                FrameId = reader["FrameID"] != DBNull.Value ? Convert.ToInt32(reader["FrameID"]) : null,
                ShotNumber = reader["ShotNumber"] != DBNull.Value ? Convert.ToInt32(reader["ShotNumber"]) : null,
                LeaveType = reader["LeaveType"] != DBNull.Value ? Convert.ToInt32(reader["LeaveType"]) : null,
                Side = reader["Side"] as string,
                Position = reader["Position"] as string,
                Comment = reader["Comment"] as string
            });
        }

        return shots;
    }
}

