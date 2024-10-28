using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void PracticeTable(Database temp)
    {
        // Practice Table
        {
            // Create new table
            var PracticeTable = new Table(temp, "Practice");

            // practice_id
            var practice_id = new Column(PracticeTable, "practice_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            PracticeTable.Columns.Add(practice_id);

            // Event Id
            var event_id = new Column(PracticeTable, "event_id", DataType.BigInt)
            {
                Nullable = false
            };
            PracticeTable.Columns.Add(event_id);

            if (!temp.Tables.Contains("Practice"))
            {
                PracticeTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [Practice] ADD CONSTRAINT PK_Practice PRIMARY KEY (practice_id);";
                temp.ExecuteNonQuery(sql);

                // Create the foreign key after the "PracticeTable" has been created
                {
                    PracticeTable = temp.Tables["Practice"]; // Retrieve the existing "PracticeTable"

                    // Event id                             Foriegn Key from Event
                    var eventIdKey = new ForeignKey(PracticeTable, "FK_Practice_Event");
                    var eventIdKeyCol = new ForeignKeyColumn(eventIdKey, "event_id")
                    {
                        ReferencedColumn = "event_id"
                    };
                    eventIdKey.Columns.Add(eventIdKeyCol);
                    eventIdKey.ReferencedTable = "Event";

                    eventIdKey.Create();
                }
            }
        }
    }
}