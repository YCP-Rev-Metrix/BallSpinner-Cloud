using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class Dbcoretest
{
    private void SmartDotListTable(Database temp)
    {
        Console.WriteLine("Creating SmartDotListTable and temp data");
        
        var smartDotListTable = new Table(temp, "SmartDotList");

        // User Id
        var userid = new Column(smartDotListTable, "userid", DataType.BigInt)
        {
            Nullable = false
        };

        smartDotListTable.Columns.Add(userid);

        // Ball Id
        var smartdotid = new Column(smartDotListTable, "smartdot_id", DataType.BigInt)
        {
            Nullable = false
        };

        smartDotListTable.Columns.Add(smartdotid);


        if (!temp.Tables.Contains("SmartDotList"))
        {
            smartDotListTable.Create();

            // Ball FK
            var smartdotIdKey = new ForeignKey(smartDotListTable, "SmartDotList_SmartDot_FK");
            var smartdotIdKeyCol = new ForeignKeyColumn(smartdotIdKey, "smartdot_id")
            {
                ReferencedColumn = "smartdot_id"
            };
            smartdotIdKey.Columns.Add(smartdotIdKeyCol);
            smartdotIdKey.ReferencedTable = "SmartDot";

            smartdotIdKey.Create();
            
            // User FK
            var userIdKey = new ForeignKey(smartDotListTable, "SmartDotList_User_FK");
            var userIdKeyCol = new ForeignKeyColumn(userIdKey, "userid")
            {
                ReferencedColumn = "id"
            };
            userIdKey.Columns.Add(userIdKeyCol);
            userIdKey.ReferencedTable = "User";

            userIdKey.Create();
            CreateDefaultSmartDotList();
            Console.WriteLine("Success");
        }
        
    }

    private void CreateDefaultSmartDotList()
    {
        string sql = "INSERT INTO [SmartDotList] (userid, smartdot_id) " +
                     "VALUES (@userid, @smartdot_id);";
        
        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@userid", 1);
                cmd.Parameters.AddWithValue("@smartdot_id", 1);

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
    
}