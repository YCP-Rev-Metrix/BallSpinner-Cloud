using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<(bool success, List<ShotTable> shots)> GetAppShots()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT ID, Type, SmartDotID, SessionID, BallID, FrameID ,ShotNumber, LeaveType, Side, Position, Comment FROM combinedDB.[Shots]"; // Adjusted to select more fields
        
        using var command = new SqlCommand(selectQuery, connection);

        var shots = new List<ShotTable>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // construct a new UserIdentification object for each row
            var shot = new ShotTable
            {
                ID = (int)reader["ID"],
                SmartDotID = (int)reader["SmartDotID"],
                Type = (int)reader["Type"],
                SessionID = (int)reader["SessionID"],
                BallID = (int)reader["BallID"],
                FrameID = (int)reader["FrameID"],
                ShotNumber = (int)reader["ShotNumber"],
                LeaveType = (int)reader["LeaveType"],
                Side = reader["Side"] as string ?? string.Empty,
                Position = reader["Position"] as string ?? string.Empty,
                Comment = reader["Comment"] as string ?? string.Empty

            };

            shots.Add(shot);
        }

        return (shots.Any(), shots);
    }
}