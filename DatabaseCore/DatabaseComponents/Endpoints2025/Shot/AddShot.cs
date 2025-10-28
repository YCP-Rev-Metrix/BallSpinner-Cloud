using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddShot(int type, int smartDotId, int sessionId, int ballId, int frameId, int shotNumber, int leaveType, string side, string position, string comment)
    {
        // If not local use Server conn string, if local use local conn string
        //ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        ConnectionString = Environment.GetEnvironmentVariable("LOCALDB_CONNECTION_STRING");
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

        string insertQuery = "INSERT INTO combinedDB.[Shots] (Type, SmartDotID, SesssionId, BallID, FrameID, ShotNumber, LeaveType, Side, Position, Comment) " +
                             "VALUES (@Type, @SmartDotID, @SesssionID, @BallID, @FrameID, @ShotNumber, @LeaveType, @Side, @Position, @Comment)";

        using var command = new SqlCommand(insertQuery, connection1);

        command.Parameters.Add("@Type", SqlDbType.Int).Value = type;
        command.Parameters.Add("@SmartDotID", SqlDbType.Int).Value = smartDotId;
        command.Parameters.Add("@SesssionID", SqlDbType.Int).Value = sessionId;
        command.Parameters.Add("@BallID", SqlDbType.Int).Value = ballId;
        command.Parameters.Add("@FrameID", SqlDbType.Int).Value = frameId;
        command.Parameters.Add("@ShotNumber", SqlDbType.Int).Value = shotNumber;
        command.Parameters.Add("@LeaveType", SqlDbType.Int).Value = leaveType;
        command.Parameters.Add("@Side", SqlDbType.VarChar).Value = side;
        command.Parameters.Add("@Position", SqlDbType.VarChar).Value = position;
        command.Parameters.Add("@Comment", SqlDbType.VarChar).Value = comment;


        // Execute the query
        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
    
}