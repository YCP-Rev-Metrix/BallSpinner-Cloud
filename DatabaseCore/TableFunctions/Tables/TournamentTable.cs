using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void TournamentTable(Database temp)
    {
        // Tournament Table
        {
            // Create new table
            var TournamentTable = new Table(temp, "Tournament");

            // tournament_id
            var tournament_id = new Column(TournamentTable, "tournament_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            TournamentTable.Columns.Add(tournament_id);

            // Event Id
            var event_id = new Column(TournamentTable, "event_id", DataType.BigInt)
            {
                Nullable = false
            };
            TournamentTable.Columns.Add(event_id);

            if (!temp.Tables.Contains("Tournament"))
            {
                TournamentTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [Tournament] ADD CONSTRAINT PK_Tournament PRIMARY KEY (tournament_id);";
                temp.ExecuteNonQuery(sql);

                // Create the foreign key after the "TournamentTable" has been created
                {
                    TournamentTable = temp.Tables["Tournament"]; // Retrieve the existing "PracticeTable"

                    // Event id                             Foriegn Key from Event
                    var eventIdKey = new ForeignKey(TournamentTable, "FK_Tournament_Event");
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