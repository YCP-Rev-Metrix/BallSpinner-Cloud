using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    private void SmartDotListTable(Database temp)
    {
        Console.WriteLine("Creating SmartDotListTable");
        
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
            
            Console.WriteLine("Success");
        }
    }
}