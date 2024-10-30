using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class 
    RevMetrixDB

{
    private void GameTable(Database temp)
    {
        // Game Table
        {
            // Create new Table
            var GameTable = new Table(temp, "Game");

            // Game id
            var game_id = new Column(GameTable, "game_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            GameTable.Columns.Add(game_id);

            // session_id
            var session_id = new Column(GameTable, "session_id", DataType.BigInt)
            {
                Nullable = false
            };
            GameTable.Columns.Add(session_id);

            var score = new Column(GameTable, "score", DataType.VarChar(255));
            GameTable.Columns.Add(score);

            if (!temp.Tables.Contains("Game"))
            {
                GameTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [Game] ADD CONSTRAINT PK_Game PRIMARY KEY (game_id);";
                temp.ExecuteNonQuery(sql);

                // Create the foreign key after the "GameTable" has been created
                {
                    GameTable = temp.Tables["Game"]; // Retrieve the existing "GameTable"

                    // Event id                             Foriegn Key from Session
                    var sessionIdKey = new ForeignKey(GameTable, "FK_Game_Session");
                    var sessionIdKeyCol = new ForeignKeyColumn(sessionIdKey, "session_id")
                    {
                        ReferencedColumn = "session_id"
                    };
                    sessionIdKey.Columns.Add(sessionIdKeyCol);
                    sessionIdKey.ReferencedTable = "Session";

                    sessionIdKey.Create();
                }

            }
        }
    }
}