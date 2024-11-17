using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{
    private void ArsenalTable(Database temp)
    {
        Console.WriteLine("Creating ArsenalTable and temp data");
        var arsenalTable = new Table(temp, "Arsenal");

        // User Id
        var userid = new Column(arsenalTable, "userid", DataType.BigInt)
        {
            Nullable = false
        };

        arsenalTable.Columns.Add(userid);

        // Ball Id
        var ballId = new Column(arsenalTable, "ball_id", DataType.BigInt)
        {
            Nullable = false
        };

        arsenalTable.Columns.Add(ballId);


        if (!temp.Tables.Contains("Arsenal"))
        {
            arsenalTable.Create();

            arsenalTable = temp.Tables["Arsenal"];

            // Ball FK
            var ballIdKey = new ForeignKey(arsenalTable, "Arsenal_Ball_FK");
            var ballIdKeyCol = new ForeignKeyColumn(ballIdKey, "ball_id")
            {
                ReferencedColumn = "ballid"
            };
            ballIdKey.Columns.Add(ballIdKeyCol);
            ballIdKey.ReferencedTable = "Ball";

            ballIdKey.Create();
            
            // User FK
            var userIdKey = new ForeignKey(arsenalTable, "Arsenal_User_FK");
            var userIdKeyCol = new ForeignKeyColumn(userIdKey, "userid")
            {
                ReferencedColumn = "id"
            };
            userIdKey.Columns.Add(userIdKeyCol);
            userIdKey.ReferencedTable = "User";

            userIdKey.Create();
            CreateDefaultArsenal();
            Console.WriteLine("Success");
        }
        
    }

    private void CreateDefaultArsenal()
    {
        string sql = "INSERT INTO [Arsenal] (userid, ball_id) " +
                     "VALUES (@userid, @ball_id);";
        
        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@userid", "1");
                cmd.Parameters.AddWithValue("@ball_id", "1");

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
    
}