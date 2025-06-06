﻿using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DatabaseCore.DatabaseComponents;

public partial class Dbcore
{   
    public Task NukeAsync()
    {
        string? ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
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
                    if (CheckTables(transaction, connection) > 0)
                    {
                        Console.WriteLine("Make sure to delete all tables before proceeding");
                        
                        // TODO
                        // There is somehthing with the env stuff which is preventing the connectionstring for the 
                        // revmetrix-bs database to be used.
                        
                        /*
                        NoContraint(transaction, connection);
                    
                        // Get all table names
                        string getTablesQuery = @"
                        SELECT TABLE_NAME 
                        FROM [revmetrix-bs].INFORMATION_SCHEMA.TABLES
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
                         Console.WriteLine("All tables have been dropped ");
                         */
                        return Task.FromException(new Exception($"Delete all tables before proceeding"));

                    }
                    else
                    {
                        Console.WriteLine("No tables exist in the database");
                        return Task.CompletedTask;

                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return Task.FromException(ex);

            }
        }

    }

    private void NoContraint(SqlTransaction transaction, SqlConnection connection)
    {
        /*
         * Remove Refresh Token Contraint
         */
        string constraint = "ALTER TABLE [revmetrix-bs].dbo.RefreshToken DROP CONSTRAINT FK_RefreshToken_User";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "Refresh-User Constraint Removed.");
        }
        
        /*
         * Remove Arsenal Constraints
         */
        constraint = "ALTER TABLE [revmetrix-bs].dbo.Arsenal DROP CONSTRAINT Arsenal_Ball_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "Arsenal-Ball Constraint Removed");
        }
        
        constraint = "ALTER TABLE [revmetrix-bs].dbo.Arsenal DROP CONSTRAINT Arsenal_User_FK";
        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "Arsenal-User Constraint Removed");
        }
        
        /*
         * Remove Simulated Shot List Constraints
         */
        constraint = "ALTER TABLE [revmetrix-bs].dbo.SimulatedShotList DROP CONSTRAINT SimulatedShotList_SimulatedShot_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "SimulatedShotList-SimulatedShot Constraint Removed");
        }
        constraint = "ALTER TABLE [revmetrix-bs].dbo.SimulatedShotList DROP CONSTRAINT SimulatedShotList_User_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "SimulatedShotList-User Constraint Removed");
        }
        
        /*
         * Remove SmartDot Sensor Constraints
         */
        constraint = "ALTER TABLE [revmetrix-bs].dbo.SD_Sensor DROP CONSTRAINT SD_Sensor_SimulatedShot_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "SmartDotSensor-SimulatedShot Constraint Removed");
        }
        
        constraint = "ALTER TABLE [revmetrix-bs].dbo.SD_Sensor DROP CONSTRAINT SD_Sensor_SensorType_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "SmartDotSensor-Type Constraint Removed");
        }
        /*
         * Remove SampleData Constraint
         */
        constraint = "ALTER TABLE [revmetrix-bs].dbo.SensorData DROP CONSTRAINT SampleData_SDSensor_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "SampleData-SDSensor Constraint Removed");
        }
        /*
         * Remove SmartDotList Constraint
         */
        constraint = "ALTER TABLE [revmetrix-bs].dbo.SmartDotList DROP CONSTRAINT SmartDotList_User_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "SmartDotList-User Constraint Removed");
        }
        
        constraint = "ALTER TABLE [revmetrix-bs].dbo.SmartDotList DROP CONSTRAINT SmartDotList_SmartDot_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "SmartDotList-SmartDot Constraint Removed");
        }
        /*
         * Remove LocalShotsTable
         */
        
        constraint = "ALTER TABLE [revmetrix-bs].dbo.LocalShots DROP CONSTRAINT LocalShot_userID_FK";

        using (SqlCommand dropTableCommand = new SqlCommand(constraint, connection, transaction))
        {
            int rows = dropTableCommand.ExecuteNonQuery();
            Console.WriteLine(rows == 0 ? "No constraint to drop." : "LocalShots-User Constraint Removed");
        }
        Console.WriteLine("");
    }

    public int CheckTables(SqlTransaction transaction, SqlConnection connection)
    {
        string query = "SELECT COUNT(*) FROM [revmetrix-bs].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
        using (SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            // Execute the query and return the count as an integer
            return (int)command.ExecuteScalar();
        }
    }
}