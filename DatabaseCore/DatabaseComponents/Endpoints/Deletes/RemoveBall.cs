using System.Data;
using System.Text;
using Common.Logging;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents
{
    public partial class RevMetrixDb
    {
        public async Task<bool> RemoveBall(string? ballName, string? username)
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

                // Delete Ball from Arsenal and Ball tables
                string deleteQuery = @"
                    UPDATE b
                    SET b.status = 0
                    FROM Ball AS b
                    INNER JOIN Arsenal AS a ON b.ArsenalID = a.ArsenalID
                    WHERE b.name = @BallName;
                    ";
                
                using var deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@BallName", ballName);

                int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();
                if (rowsAffected <= 0)
                {
                    throw new InvalidOperationException($"Failed to delete ball '{ballName}' for user '{username}'.");
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
