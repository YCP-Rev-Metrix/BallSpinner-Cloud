using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class Dbcore
{
    private void SimulatedShotListTable(Database temp)
    {
        Console.WriteLine("Creating SimulatedShotListTable and temp data");
        var simulatedShotListTable = new Table(temp, "SimulatedShotList");

        // User Id
        var userid = new Column(simulatedShotListTable, "userid", DataType.BigInt)
        {
            Nullable = false
        };

        simulatedShotListTable.Columns.Add(userid);

        // Shot Id
        var shotId = new Column(simulatedShotListTable, "shotid", DataType.BigInt)
        {
            Nullable = false
        };

        simulatedShotListTable.Columns.Add(shotId);
        
        var name = new Column(simulatedShotListTable, "name", DataType.VarChar(30))
        {
            Nullable = true
        };
        simulatedShotListTable.Columns.Add(name);


        if (!temp.Tables.Contains("SimulatedShotList"))
        {
            simulatedShotListTable.Create();

            simulatedShotListTable = temp.Tables["SimulatedShotList"];

            // SimulatedShot FK
            var shotIdKey = new ForeignKey(simulatedShotListTable, "SimulatedShotList_SimulatedShot_FK");
            var shotIdKeyCol = new ForeignKeyColumn(shotIdKey, "shotid")
            {
                ReferencedColumn = "shotid"
            };
            shotIdKey.Columns.Add(shotIdKeyCol);
            shotIdKey.ReferencedTable = "SimulatedShot";

            shotIdKey.Create();
            
            // User FK
            var userIdKey = new ForeignKey(simulatedShotListTable, "SimulatedShotList_User_FK");
            var userIdKeyCol = new ForeignKeyColumn(userIdKey, "userid")
            {
                ReferencedColumn = "id"
            };
            userIdKey.Columns.Add(userIdKeyCol);
            userIdKey.ReferencedTable = "User";

            var sql = "ALTER TABLE [SimulatedShotList] ADD CONSTRAINT SimulatedShotList_UserName_UNIQUE UNIQUE (userid, name);";
            temp.ExecuteNonQuery(sql);

            userIdKey.Create();
            Console.WriteLine("Success");
        }
        
    }
}