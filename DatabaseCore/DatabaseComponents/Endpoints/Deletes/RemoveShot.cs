using System.Data;
using System.Text;
using Common.Logging;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents
{
    public partial class RevMetrixDb
    {
        public async Task<bool> RemoveShot(string shotname, string? username)
        {
            try
            {
                // Validate connection string
                ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new InvalidOperationException("Connection string is not set.");
                }

                // Open the connection
                await using var connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();

                // Get User Id
                int userId = await GetUserId(username);
                if (userId <= 0)
                {
                    throw new ArgumentException("Invalid user ID.");
                }
                /*
                 * DELETE FROM [Ball]
                   FROM [dbo].[Ball]
                   INNER JOIN [dbo].[Arsenal] ON [dbo].[Arsenal].ball_id = [dbo].[Ball].ballid
                   INNER JOIN [dbo].[User] ON [dbo].[User].id = [dbo].[Arsenal].userid
                   WHERE [dbo].[Ball].name = 'testing12' AND [dbo].[User].id = 1;
                   
                 */
                // Delete shot
                string deleteQuery = @"
                    DELETE FROM ss
                    FROM SimulatedShot as ss
                    INNER JOIN SimulatedShotList AS ssl ON ss.ShotId = ssl.ShotId
                    INNER JOIN SD_Sensor as sds ON sds.shotid = ss.shotid
                    INNER JOIN SensorType as st ON  st.type_id = sds.type_id
                    INNER JOIN SensorData as sd ON sd.sensor_id = sds.sensor_id
                    INNER JOIN [User] U on U.id = ssl.userid
                    WHERE U.id = @userId AND ssl.name = @shotname;
                    ";
                
                using var deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@shotname", shotname);
                deleteCommand.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();
                if (rowsAffected <= 0)
                {
                    throw new InvalidOperationException($"Failed to delete shot '{shotname}' for user '{username}'.");
                }

                return true;
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exceptions
                LogWriter.LogInfo(sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Log general exceptions
                LogWriter.LogError(ex.Message);
                return false;
            }
        }
    }
}
