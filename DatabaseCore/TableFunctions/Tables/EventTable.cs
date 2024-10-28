using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void EventTable(Database temp)
    {
        // Event Table
        {
            // Create new Table
            var EventTable = new Table(temp, "Event");

            // Game id
            var event_id = new Column(EventTable, "event_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            EventTable.Columns.Add(event_id);

            // User ID
            var userId = new Column(EventTable, "userid", DataType.BigInt)
            {
                Nullable = false
            };
            EventTable.Columns.Add(userId);

            // Event Type
            var event_type = new Column(EventTable, "event_type", DataType.VarChar(50));
            EventTable.Columns.Add(event_type);

            if (!temp.Tables.Contains("Event"))
            {
                EventTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [Event] ADD CONSTRAINT PK_Event PRIMARY KEY (event_id);";
                temp.ExecuteNonQuery(sql);

                // Create the foreign key after the "EventTable" has been created
                {
                    EventTable = temp.Tables["Event"]; // Retrieve the existing "EventTable"

                    // User ID
                    var userIdKey = new ForeignKey(EventTable, "FK_Event_User");
                    var userIdKeyCol = new ForeignKeyColumn(userIdKey, "userid")
                    {
                        ReferencedColumn = "id"
                    };
                    userIdKey.Columns.Add(userIdKeyCol);
                    userIdKey.ReferencedTable = "User";

                    userIdKey.Create();
                }
            }
        }
    }
}