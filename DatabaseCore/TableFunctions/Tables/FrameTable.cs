using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void FrameTable(Database temp)
    {
        // Frame Table
        {
            // Create new
            var FrameTable = new Table(temp, "Frame");

            // Frame_id
            var frame_id = new Column(FrameTable, "frame_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            FrameTable.Columns.Add(frame_id);

            // User ID
            var game_id = new Column(FrameTable, "game_id", DataType.BigInt)
            {
                Nullable = false
            };
            FrameTable.Columns.Add(game_id);

            // Shot_number
            var shot_number = new Column(FrameTable, "shot_number", DataType.BigInt)
            {
                Nullable = false
            };
            FrameTable.Columns.Add(shot_number);

            var score = new Column(FrameTable, "score", DataType.VarChar(255));
            FrameTable.Columns.Add(score);

            if (!temp.Tables.Contains("Frame")) 
            {
                FrameTable.Create();
            }

            // Create the primary key constraint using SQL
            string sql = "ALTER TABLE [Frame] ADD CONSTRAINT PK_Frame PRIMARY KEY (frame_id);";
            temp.ExecuteNonQuery(sql);

            // Create the foreign key after the "FrameTable" has been created
            {
                FrameTable = temp.Tables["Frame"]; // Retrieve the existing "FrameTable"

                // Game id                             Foriegn Key from Game
                var gameIdKey = new ForeignKey(FrameTable, "FK_Frame_Game");
                var gameIdKeyCol = new ForeignKeyColumn(gameIdKey, "game_id")
                {
                    ReferencedColumn = "game_id"
                };
                gameIdKey.Columns.Add(gameIdKeyCol);
                gameIdKey.ReferencedTable = "Game";

                gameIdKey.Create();
            }

        }
    }
}