using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void ArsenalTable(Database temp)
    {
        // Arsenal Table
        {
            var ArsenalTable = new Table(temp, "Arsenal");

            var userid = new Column(ArsenalTable, "userid", DataType.BigInt)
            {
                Nullable = false
            };
            ArsenalTable.Columns.Add(userid);

            var ball_id = new Column(ArsenalTable, "ball_id", DataType.BigInt)
            {
                Nullable = false
            };
            ArsenalTable.Columns.Add(ball_id);

            // Check if the Arsenal table already exists
            if (!temp.Tables.Contains("Arsenal"))
            {
                // TODO: Fix this primary key 
                // Create a primary key for the Arsenal table
                //var primaryKey = new PrimaryKey(ArsenalTable, "PK_Arsenal", new[] { userid.Name, ball_id.Name });
                //ArsenalTable.Keys.Add(primaryKey);

                // Optionally add foreign key constraints if necessary
                var userIdKey = new ForeignKey(ArsenalTable, "FK_Arsenal_User");
                var userIdKeyCol = new ForeignKeyColumn(userIdKey, "userid")
                {
                    ReferencedColumn = "id"
                };
                
                userIdKey.Columns.Add(userIdKeyCol);
                userIdKey.ReferencedTable = "User";
                userIdKey.Create();
                
                var ballIdKey = new ForeignKey(ArsenalTable, "FK_Arsenal_Ball");
                var ballIdKeyCol = new ForeignKeyColumn(ballIdKey, "ball_id")
                {
                    ReferencedColumn = "ballid"
                };
                userIdKey.Columns.Add(ballIdKeyCol);
                userIdKey.ReferencedTable = "Ball";
                userIdKey.Create();
                // Add the Arsenal table to the database
                temp.Tables.Add(ArsenalTable);
            }
        }
    }
}