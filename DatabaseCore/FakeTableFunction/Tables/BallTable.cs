using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{
    private void BallTable(Database temp)
    {
        Console.WriteLine("Creating BallTable and temp data");
        var ballTable = new Table(temp, "Ball");

        // Ball Id
        var ballid = new Column(ballTable, "ballid", DataType.BigInt)
        {
            IdentityIncrement = 1,
            Nullable = false,
            IdentitySeed = 1,
            Identity = true
        };

        ballTable.Columns.Add(ballid);

        var name = new Column(ballTable, "name", DataType.VarChar(100))
        {
            Nullable = false
        };

        ballTable.Columns.Add(name);

        var weight = new Column(ballTable, "weight", DataType.Float)
        {
            Nullable = false
        };

        ballTable.Columns.Add(weight);

        var diameter = new Column(ballTable, "diameter", DataType.Float)
        {
            Nullable = false
        };
        
        ballTable.Columns.Add(diameter);

        var coretype = new Column(ballTable, "core_type", DataType.VarChar(25))
        {
            Nullable = true
        };

        ballTable.Columns.Add(coretype);

        if (!temp.Tables.Contains("Ball"))
        {
            ballTable.Create();

            string sql = "ALTER TABLE [Ball] ADD CONSTRAINT Ball_PK PRIMARY KEY (ballid);";
            temp.ExecuteNonQuery(sql);
            
            sql = "ALTER TABLE [Ball] ADD CONSTRAINT BallName_UNIQUE UNIQUE (name);";
            temp.ExecuteNonQuery(sql);

            CreateDefaultBall();
            Console.WriteLine("Success");
        }
        
    }

    private void CreateDefaultBall()
    {
        string sql = "INSERT INTO [Ball] (name, weight, diameter, core_type) " +
                     "VALUES (@name, @weight, @diameter, @core_type);";
        
        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@name", "string");
                cmd.Parameters.AddWithValue("@weight", 12);
                cmd.Parameters.AddWithValue("@core_type", "string");  
                cmd.Parameters.AddWithValue("@diameter", 20);

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }

}