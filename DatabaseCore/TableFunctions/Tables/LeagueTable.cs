using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void LeagueTable(Database temp)
    {
        // League Table
        {
            // Create new table
            var LeagueTable = new Table(temp, "League");

            // league_id
            var league_id = new Column(LeagueTable, "league_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            LeagueTable.Columns.Add(league_id);

            // Event Id
            var event_id = new Column(LeagueTable, "event_id", DataType.BigInt)
            {
                Nullable = false
            };
            LeagueTable.Columns.Add(event_id);

            if (!temp.Tables.Contains("League"))
            {
                LeagueTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [League] ADD CONSTRAINT PK_League PRIMARY KEY (league_id);";
                temp.ExecuteNonQuery(sql);

                // Create the foreign key after the "LeagueTable" has been created
                {
                    LeagueTable = temp.Tables["League"]; // Retrieve the existing "LeagueTable"

                    // Event id                             Foriegn Key from Event
                    var eventIdKey = new ForeignKey(LeagueTable, "FK_League_Event");
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