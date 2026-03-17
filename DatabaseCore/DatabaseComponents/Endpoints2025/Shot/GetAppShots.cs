using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<Shot> shots)> GetAppShots()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID, Type, SmartDotID, SessionID, BallID, FrameID, ShotNumber, LeaveType, Side, Position, Comment FROM combinedDB.[Shots]";
        
        using var command = new SqlCommand(selectQuery, connection);

        var shots = new List<Shot>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var shot = new Shot
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
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
            };

            shots.Add(shot);
        }

        return (shots.Any(), shots);
    }
}