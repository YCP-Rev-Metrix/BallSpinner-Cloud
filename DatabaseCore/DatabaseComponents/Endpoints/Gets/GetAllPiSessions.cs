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

      // Convert yyyymmddhhmmss integer to DateTime (start of day)
      if (rangeStart > 0)
      {
         try
         {
            double yearDouble = rangeStart * (0.0000000001);
            int y = (int)Math.Floor(yearDouble);

            double monthDouble = rangeStart * (0.00000001) - (y * 100);
            int m = (int)Math.Floor(monthDouble);
            
            double dayDouble = rangeStart * (0.000001) - (y * 10000) - (m * 100);
            int d = (int)Math.Floor(dayDouble);
            
            double hourDouble = rangeStart * (0.0001) - (y * 1000000) - (m * 10000) - (d * 100);
            int h = (int)Math.Floor(hourDouble);
            
            double minuteDouble = rangeStart * (0.01) - (y * 100000000) - (m * 1000000) - (d * 10000) - (h * 100);
            int min = (int)Math.Floor(minuteDouble);
            
            double secondDouble = rangeStart * (1) - (y * 10000000000) - (m * 100000000) - (d * 1000000) - (h * 10000) - (min * 100);
            int sec = (int)Math.Floor(secondDouble);
            
            startDate = new DateTime(y, m, d, h, min, sec, DateTimeKind.Utc);
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
            double yearDouble = rangeStart * (0.0000000001);
            int y = (int)Math.Floor(yearDouble);

            double monthDouble = rangeStart * (0.00000001) - (y * 100);
            int m = (int)Math.Floor(monthDouble);
            
            double dayDouble = rangeStart * (0.000001) - (y * 10000) - (m * 100);
            int d = (int)Math.Floor(dayDouble);
            
            double hourDouble = rangeStart * (0.0001) - (y * 1000000) - (m * 10000) - (d * 100);
            int h = (int)Math.Floor(hourDouble);
            
            double minuteDouble = rangeStart * (0.01) - (y * 100000000) - (m * 1000000) - (d * 10000) - (h * 100);
            int min = (int)Math.Floor(minuteDouble);
            
            double secondDouble = rangeStart * (1) - (y * 10000000000) - (m * 100000000) - (d * 1000000) - (h * 10000) - (min * 100);
            int sec = (int)Math.Floor(secondDouble);
            // end of day inclusive
            endDate = new DateTime(y, m, d, h, min, sec, DateTimeKind.Utc);
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
