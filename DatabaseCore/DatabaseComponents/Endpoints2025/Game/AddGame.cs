using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddGame(Game game)
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

        string insertQuery = "INSERT INTO [combinedDB].[Games] (GameNumber, Lanes, Score, Win, StartingLane, SessionID, TeamResult, IndividualResult, MobileID) " +
                             "VALUES (@GameNumber, @Lanes, @Score, @Win, @StartingLane, @SessionID, @TeamResult, @IndividualResult, @MobileID)";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@GameNumber", SqlDbType.VarChar).Value = game.GameNumber ?? string.Empty;
        command.Parameters.Add("@Lanes", SqlDbType.VarChar).Value = game.Lanes ?? string.Empty;
        command.Parameters.Add("@Score", SqlDbType.Int).Value = game.Score ?? (object)DBNull.Value;
        command.Parameters.Add("@Win", SqlDbType.Int).Value = game.Win ?? (object)DBNull.Value;
        command.Parameters.Add("@StartingLane", SqlDbType.Int).Value = game.StartingLane ?? (object)DBNull.Value;
        command.Parameters.Add("@SessionID", SqlDbType.Int).Value = game.SessionId ?? (object)DBNull.Value;
        command.Parameters.Add("@TeamResult", SqlDbType.Int).Value = game.TeamResult ?? (object)DBNull.Value;
        command.Parameters.Add("@IndividualResult", SqlDbType.Int).Value = game.IndividualResult ?? (object)DBNull.Value;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = game.MobileID.HasValue ? (object)game.MobileID.Value : DBNull.Value;

        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
}
