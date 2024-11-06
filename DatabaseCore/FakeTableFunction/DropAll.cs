using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{   
    public Task NukeAsync()
    {
        string? ConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");
        // Define your connection string (use your own credentials and server details)

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            try
            {
                // Open the connection to the database
                connection.Open();

                // Start a transaction to ensure atomicity
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    string noConstraint = "IF OBJECT_ID('FK_RefreshToken_User', 'F') IS NOT NULL ALTER TABLE [revmetrix-test].dbo.RefreshToken DROP CONSTRAINT FK_RefreshToken_User";

                    using (SqlCommand dropTableCommand = new SqlCommand(noConstraint, connection, transaction))
                    {
                        int rows = dropTableCommand.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            Console.WriteLine("No constraint to drop.");

                        }
                        else
                        {
                            Console.WriteLine("Refresh Constraint Removed");
                        }
                    }
                    
                    // Get all table names
                    string getTablesQuery = @"
                        SELECT TABLE_NAME 
                        FROM [revmetrix-test].INFORMATION_SCHEMA.TABLES
                        WHERE TABLE_TYPE = 'BASE TABLE'";

                    SqlCommand getTablesCommand = new SqlCommand(getTablesQuery, connection, transaction);
                    SqlDataAdapter adapter = new SqlDataAdapter(getTablesCommand);
                    DataTable tables = new DataTable();
                    adapter.Fill(tables);

                    // Loop through the table names and drop each one
                    foreach (DataRow row in tables.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        string dropTableQuery = $"DROP TABLE [{tableName}]";

                        using (SqlCommand dropTableCommand = new SqlCommand(dropTableQuery, connection, transaction))
                        {
                            dropTableCommand.ExecuteNonQuery();
                            Console.WriteLine($"Dropped table: {tableName}");
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                    Console.WriteLine("All tables have been dropped successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        return Task.CompletedTask;
    }

}
