using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> AddSession(Session session)
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

        string insertQuery = "INSERT INTO [combinedDB].[Sessions] (SessionNumber, EstablishmentID, EventID, DateTime, TeamOpponent, IndividualOpponent, Score, Stats, TeamRecord, IndividualRecord, MobileID) " +
                             "VALUES (@SessionNumber, @EstablishmentID, @EventID, @DateTime, @TeamOpponent, @IndividualOpponent, @Score, @Stats, @TeamRecord, @IndividualRecord, @MobileID)";

        using var command = new SqlCommand(insertQuery, connection);
        command.Parameters.Add("@SessionNumber", SqlDbType.Int).Value = session.SessionNumber ?? (object)DBNull.Value;
        command.Parameters.Add("@EstablishmentID", SqlDbType.Int).Value = session.EstablishmentId ?? (object)DBNull.Value;
        command.Parameters.Add("@EventID", SqlDbType.Int).Value = session.EventId ?? (object)DBNull.Value;
        command.Parameters.Add("@DateTime", SqlDbType.Int).Value = session.DateTime ?? (object)DBNull.Value;
        command.Parameters.Add("@TeamOpponent", SqlDbType.VarChar).Value = session.TeamOpponent ?? string.Empty;
        command.Parameters.Add("@IndividualOpponent", SqlDbType.VarChar).Value = session.IndividualOpponent ?? string.Empty;
        command.Parameters.Add("@Score", SqlDbType.Int).Value = session.Score ?? (object)DBNull.Value;
        command.Parameters.Add("@Stats", SqlDbType.Int).Value = session.Stats ?? (object)DBNull.Value;
        command.Parameters.Add("@TeamRecord", SqlDbType.Int).Value = session.TeamRecord ?? (object)DBNull.Value;
        command.Parameters.Add("@IndividualRecord", SqlDbType.Int).Value = session.IndividualRecord ?? (object)DBNull.Value;
        command.Parameters.Add("@MobileID", SqlDbType.Int).Value = session.MobileID.HasValue ? (object)session.MobileID.Value : DBNull.Value;

        int i = await command.ExecuteNonQueryAsync();
        return i > 0;
    }
}
