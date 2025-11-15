using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
   public async Task<List<PiSession>> GetAllPiSessions(DateTime? rangeStart, DateTime? rangeEnd)
   {
      ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
      using var connection = new SqlConnection(ConnectionString);
      await connection.OpenAsync();

      if (rangeStart == DateTime.MinValue)
      {
         rangeStart = null;
      }
      
      if (rangeEnd == DateTime.MinValue)
      {
         rangeEnd = null;
      }

      string selectQuery = @"
      SELECT b.id, b.name, b.timeStamp, b.isShotMode
      FROM [Team_PI_Tables].[PiSession] b
      WHERE (@start IS NULL OR b.timeStamp >= @start)
        AND (@end IS NULL OR b.timeStamp <= @end);";

      using var command = new SqlCommand(selectQuery, connection);
      var startParam = command.Parameters.Add("@start", System.Data.SqlDbType.DateTime2);
      startParam.Value = (object?)rangeStart ?? DBNull.Value;
      var endParam = command.Parameters.Add("@end", System.Data.SqlDbType.DateTime2);
      endParam.Value = (object?)rangeEnd ?? DBNull.Value;

      using SqlDataReader reader = await command.ExecuteReaderAsync();
      
      var sessions = new List<PiSession>();
      
      while (await reader.ReadAsync())
      {
         var fetchedSessions = new PiSession
         {
            Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
            Name = reader["name"] as string,
            TimeStamp = reader["timeStamp"] != DBNull.Value ? Convert.ToDateTime(reader["timeStamp"]) : null,
            IsShotMode = reader["isShotMode"] != DBNull.Value ? Convert.ToBoolean(reader["isShotMode"]) : null
         };
         sessions.Add(fetchedSessions);
      }
      return sessions;
   } 
}
