using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{
    private void SmartDotTable(Database temp)
    {
        Console.WriteLine("Creating SmartDotTable and temp data");
        var smartDotTable = new Table(temp, "SmartDot");
        
        var smartdotid = new Column(smartDotTable, "smartdot_id", DataType.BigInt)
        {
            IdentityIncrement = 1,
            Nullable = false,
            IdentitySeed = 1,
            Identity = true
        };

        smartDotTable.Columns.Add(smartdotid);

        var name = new Column(smartDotTable, "name", DataType.VarChar(100))
        {
            Nullable = false
        };

        smartDotTable.Columns.Add(name);

        var weight = new Column(smartDotTable, "address", DataType.VarChar(48))
        {
            Nullable = false
        };

        smartDotTable.Columns.Add(weight);


        if (!temp.Tables.Contains("SmartDot"))
        {
            smartDotTable.Create();

            string sql = "ALTER TABLE [SmartDot] ADD CONSTRAINT SmartDot_PK PRIMARY KEY (smartdot_id);";
            temp.ExecuteNonQuery(sql);
            
            sql = "ALTER TABLE [SmartDot] ADD CONSTRAINT SmartDot_UNIQUE UNIQUE (name);";
            temp.ExecuteNonQuery(sql);

            CreateDefaultSmartDot();
            Console.WriteLine("Success");
        }
        
    }

    private void CreateDefaultSmartDot()
    {
        string sql = "INSERT INTO [SmartDot] (name, address) " +
                     "VALUES (@name, @address)";
        
        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@name", "string");
                cmd.Parameters.AddWithValue("@address", "12:14:12:12:12:93");

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }

}