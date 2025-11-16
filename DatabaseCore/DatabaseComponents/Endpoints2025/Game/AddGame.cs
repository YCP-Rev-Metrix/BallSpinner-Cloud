using Common.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.XEvent;
using System.Data;
using System.Diagnostics;
using System.IO.Pipelines;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddGame(string gameNumber, string lanes, int score, int win, int startingLane, int sessionID, int teamResult, int individualResult)
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

        string insertQuery = "INSERT INTO combinedDB.[Games] (GameNumber, Lanes, Score, Win, StartingLane, SessionID, TeamResult, IndividualResult) " +
                             "VALUES (@GameNumber, @Lanes, @Score, @Win, @StartingLane, @SessionID, @TeamResult, @IndividualResult)";

        using var command = new SqlCommand(insertQuery, connection1);

        command.Parameters.Add("@GameNumber", SqlDbType.VarChar).Value = gameNumber;
        command.Parameters.Add("@Lanes", SqlDbType.VarChar).Value = lanes;
        command.Parameters.Add("@Score", SqlDbType.Int).Value = score;
        command.Parameters.Add("@Win", SqlDbType.Int).Value = win;
        command.Parameters.Add("@StartingLane", SqlDbType.Int).Value = startingLane;
        command.Parameters.Add("@SessionID", SqlDbType.Int).Value = sessionID;
        command.Parameters.Add("@TeamResult", SqlDbType.Int).Value = teamResult;
        command.Parameters.Add("@IndividualResult", SqlDbType.Int).Value = individualResult;

        // Execute the query
        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
}