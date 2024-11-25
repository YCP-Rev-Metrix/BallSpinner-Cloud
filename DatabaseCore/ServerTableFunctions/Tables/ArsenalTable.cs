using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    private void ArsenalTable(Database temp)
    {
        Console.WriteLine("Creating ArsenalTable");
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
            Console.WriteLine("Success");
        }
        
    }
    
}