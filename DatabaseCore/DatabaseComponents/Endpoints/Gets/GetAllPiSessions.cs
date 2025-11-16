using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
   public async Task<List<PiSession>> GetAllPiSessions(int rangeStart, int rangeEnd)
   {
      ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
      using var connection = new SqlConnection(ConnectionString);
      await connection.OpenAsync();

      DateTime? startDate = null;
      DateTime? endDate = null;

      // Convert yyyymmdd integer to DateTime (start of day)
      if (rangeStart > 0)
      {
         try
         {
            int y = rangeStart / 10000;
            int m = (rangeStart / 100) % 100;
            int d = rangeStart % 100;
            startDate = new DateTime(y, m, d, 0, 0, 0, DateTimeKind.Utc);
         }
         catch
         {
            startDate = null; // invalid input -> treat as no bound
         }
      }

      // Convert yyyymmdd integer to DateTime (end of day)
      if (rangeEnd > 0)
      {
         try
         {
            int y = rangeEnd / 10000;
            int m = (rangeEnd / 100) % 100;
            int d = rangeEnd % 100;
            // end of day inclusive
            endDate = new DateTime(y, m, d, 23, 59, 59, 999, DateTimeKind.Utc);
         }
         catch
         {
            endDate = null; // invalid input -> treat as no bound
         }
      }

      string selectQuery = @"
      SELECT b.id, b.name, b.timeStamp, b.isShotMode
      FROM [Team_PI_Tables].[PiSession] b
      WHERE (@start IS NULL OR b.timeStamp >= @start)
      AND (@end IS NULL OR b.timeStamp <= @end);";

      using var command = new SqlCommand(selectQuery, connection);
      var startParam = command.Parameters.Add("@start", System.Data.SqlDbType.DateTime2);
      startParam.Value = startDate.HasValue ? (object)startDate.Value : DBNull.Value;
      var endParam = command.Parameters.Add("@end", System.Data.SqlDbType.DateTime2);
      endParam.Value = endDate.HasValue ? (object)endDate.Value : DBNull.Value;

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
