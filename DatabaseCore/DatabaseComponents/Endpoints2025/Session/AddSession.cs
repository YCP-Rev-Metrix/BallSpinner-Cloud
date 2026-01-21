using Common.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddSession(int sessionNumber, int establishmentID, int eventID, int dateTime, string teamOpponent, string individualOpponent, int score, int stats, int teamRecord, int individualRecord)
    {
        // If not local use Server conn string, if local use local conn string
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        //ConnectionString = Environment.GetEnvironmentVariable("LOCALDB_CONNECTION_STRING");
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

        string insertQuery = "INSERT INTO [combinedDB].[Sessions] (SessionNumber, EstablishmentID, EventID, DateTime, TeamOpponent, IndividualOpponent, Score,Stats, TeamRecord, IndividualRecord) " +
                             "VALUES (@SessionNumber, @EstablishmentID, @EventID, @DateTime, @TeamOpponent, @IndividualOpponent, @Score, @Stats, @TeamRecord, @IndividualRecord)";

        using var command = new SqlCommand(insertQuery, connection1);
        
        command.Parameters.Add("@SessionNumber", SqlDbType.Int).Value = sessionNumber;
        command.Parameters.Add("@EstablishmentID", SqlDbType.Int).Value = establishmentID;
        command.Parameters.Add("@EventID", SqlDbType.Int).Value = eventID;
        command.Parameters.Add("@DateTime", SqlDbType.Int).Value = dateTime;
        command.Parameters.Add("@TeamOpponent", SqlDbType.VarChar).Value = teamOpponent;
        command.Parameters.Add("@IndividualOpponent", SqlDbType.VarChar).Value = individualOpponent;
        command.Parameters.Add("@Score", SqlDbType.Int).Value = score;
        command.Parameters.Add("@Stats", SqlDbType.Int).Value = stats;
        command.Parameters.Add("@TeamRecord", SqlDbType.Int).Value = teamRecord;
        command.Parameters.Add("@IndividualRecord", SqlDbType.Int).Value = individualRecord;


        // Execute the query
        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
}