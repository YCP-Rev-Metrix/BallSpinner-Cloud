using Common.POCOs.PITeam2025;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
   public async Task<List<PiSession>> GetAllPiSessions(String rangeStart, String rangeEnd)
   {
      ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
      using var connection = new SqlConnection(ConnectionString);
      await connection.OpenAsync();

      DateTime? startDate = null;
      DateTime? endDate = null;

      // Convert yyyymmddhhmmss integer to DateTime (start of day)
      if (rangeStart != "00000000000000")
      {
         if (TryParseYmdhms(rangeStart, out var parsedDate))
         {
            startDate = parsedDate;
         }
         else
         {
            startDate = null; // invalid input -> treat as no bound
         }
      }

      // Convert yyyymmddhhmmss integer to DateTime (end of day)
      if (rangeEnd != "00000000000000")
      {
         if (TryParseYmdhms(rangeEnd, out var parsedDate))
         {
            endDate = parsedDate;
         }
         else
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
   
   private static bool TryParseYmdhms(string input, out DateTime result)
   {
      result = default;
      if (string.IsNullOrWhiteSpace(input) || input.Length != 14)
         return false;

    
      for (int i = 0; i < 14; i++)
         if (!char.IsDigit(input[i]))
            return false;

   
      if (!int.TryParse(input.Substring(0, 4), out int year) ||
          !int.TryParse(input.Substring(4, 2), out int month) ||
          !int.TryParse(input.Substring(6, 2), out int day) ||
          !int.TryParse(input.Substring(8, 2), out int hour) ||
          !int.TryParse(input.Substring(10, 2), out int minute) ||
          !int.TryParse(input.Substring(12, 2), out int second))
      {
         return false;
      }

      try
      {
         result = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
         Console.WriteLine($"Parsed DateTime: {result}");
         return true;
      }
      catch
      {
         return false;
      }
   }
}




