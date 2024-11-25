using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
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

            userIdKey.Create();

            Console.WriteLine("Success");
        }
        
    }
}