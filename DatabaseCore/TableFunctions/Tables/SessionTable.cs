using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void SessionTable(Database temp)
    {
        // Session Table
        {
            // Create new Table
            var SessionTable = new Table(temp, "Session");

            // Session ID
            var session_id = new Column(SessionTable, "session_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            SessionTable.Columns.Add(session_id);

            // league_id
            var league_id = new Column(SessionTable, "league_id", DataType.BigInt)
            {
                Nullable = true
            };
            SessionTable.Columns.Add(league_id);

            // tournament_id
            var tournament_id = new Column(SessionTable, "tournament_id", DataType.BigInt)
            {
                Nullable = true
            };
            SessionTable.Columns.Add(tournament_id);

            // practice_id
            var practice_id = new Column(SessionTable, "practice_id", DataType.BigInt)
            {
                Nullable = true
            };
            SessionTable.Columns.Add(practice_id);

            // Date
            var date = new Column(SessionTable, "date", DataType.DateTime)
            {
                Nullable = false
            };
            SessionTable.Columns.Add(date);

            // Location
            var location = new Column(SessionTable, "location", DataType.VarChar(255));
            SessionTable.Columns.Add(location);

            // Total Games
            var total_games = new Column(SessionTable, "total_games", DataType.BigInt);
            SessionTable.Columns.Add(total_games);

            // Total Frames
            var total_frames = new Column(SessionTable, "total_frames", DataType.BigInt);
            SessionTable.Columns.Add(total_frames);

            if (!temp.Tables.Contains("Session"))
            {
                SessionTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [Session] ADD CONSTRAINT PK_Session PRIMARY KEY (session_id);";
                temp.ExecuteNonQuery(sql);

                // Create the foreign keys after the "SessionTable" has been created
                {
                    SessionTable = temp.Tables["Session"]; // Retrieve the existing "SessionTable"

                    // league_id                            Foriegn Key from League
                    var leagueIdKey = new ForeignKey(SessionTable, "FK_Session_League");
                    var leagueIdKeyCol = new ForeignKeyColumn(leagueIdKey, "league_id")
                    {
                        ReferencedColumn = "league_id"
                    };
                    leagueIdKey.Columns.Add(leagueIdKeyCol);
                    leagueIdKey.ReferencedTable = "League";

                    leagueIdKey.Create();
                    //////////
                    ///
                    // tournament_id                            Foriegn Key from Tournament
                    var tournamentIdKey = new ForeignKey(SessionTable, "FK_Session_Tournament");
                    var tournamentIdKeyCol = new ForeignKeyColumn(tournamentIdKey, "tournament_id")
                    {
                        ReferencedColumn = "tournament_id"
                    };
                    tournamentIdKey.Columns.Add(tournamentIdKeyCol);
                    tournamentIdKey.ReferencedTable = "Tournament";

                    tournamentIdKey.Create();
                    //////////
                    ///
                    // Practice_id                            Foriegn Key from Practice
                    var practiceIdKey = new ForeignKey(SessionTable, "FK_Session_Practice");
                    var practiceIdKeyCol = new ForeignKeyColumn(practiceIdKey, "practice_id")
                    {
                        ReferencedColumn = "practice_id"
                    };
                    practiceIdKey.Columns.Add(practiceIdKeyCol);
                    practiceIdKey.ReferencedTable = "Practice";

                    practiceIdKey.Create();

                }

            }

        }
    }
}