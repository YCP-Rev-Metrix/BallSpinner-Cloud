using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void ShotTable(Database temp)
    {
        // Shot Table
        {
            // Create new
            var ShotTable = new Table(temp, "Shot");

            // Shot_id Primary Key 
            // Primary Key is added after the table is created. 
            var shot_id = new Column(ShotTable, "shot_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };

            ShotTable.Columns.Add(shot_id);

            // User_id which is a Foreign Key
            // Foreign Key is added after the table is created.
            var user_id = new Column(ShotTable, "user_id", DataType.BigInt)
            {
                Nullable = false
            };
            ShotTable.Columns.Add(user_id);

            // Frame_id which is a Foreign Key
            // Foreign Key is added after the table is created.
            var frame_id = new Column(ShotTable, "frame_id", DataType.BigInt)
            {
                Nullable = true
            };
            ShotTable.Columns.Add(frame_id);

            // Ball_id which is a Foreign Key
            // Foreign Key is added after the table is created.
            var ball_id = new Column(ShotTable, "ball_id", DataType.BigInt)
            {
                Nullable = true
            };
            ShotTable.Columns.Add(ball_id);

            // Video_id which is a Foreign Key
            // Foreign Key is added after the table is created.
            var video_id = new Column(ShotTable, "video_id", DataType.BigInt)
            {
                Nullable = true
            };
            ShotTable.Columns.Add(video_id);

            // Pins Remaining  
            var pins_remaining = new Column(ShotTable, "pins_remaining", DataType.BigInt)
            {
                Nullable = false
            };
            ShotTable.Columns.Add(pins_remaining);

            // Time Remaining  
            var time = new Column(ShotTable, "time", DataType.DateTime)
            {
                Nullable = false
            };
            ShotTable.Columns.Add(time);

            // Lane Number  
            var lane_Number = new Column(ShotTable, "lane_Number", DataType.BigInt)
            {
                Nullable = false
            };
            ShotTable.Columns.Add(lane_Number);

            // These will need to be updated somehow to be lists
            var ddx = new Column(ShotTable, "ddx", DataType.Float);
            ShotTable.Columns.Add(ddx);

            // These will need to be updated somehow to be lists
            var ddy = new Column(ShotTable, "ddy", DataType.Float);
            ShotTable.Columns.Add(ddy);

            // These will need to be updated somehow to be lists
            var ddz = new Column(ShotTable, "ddz", DataType.Float);
            ShotTable.Columns.Add(ddz);

            // These will need to be updated somehow to be lists
            var x_position = new Column(ShotTable, "x_position", DataType.Float);
            ShotTable.Columns.Add(x_position);

            // These will need to be updated somehow to be lists
            var y_position = new Column(ShotTable, "y_position", DataType.Float);
            ShotTable.Columns.Add(y_position);

            // These will need to be updated somehow to be lists
            var z_position = new Column(ShotTable, "z_position", DataType.Float);
            ShotTable.Columns.Add(z_position);

            var pocket_hit = new Column(ShotTable, "pocket_hit", DataType.BigInt);
            ShotTable.Columns.Add(pocket_hit);

            // Create the entire table for shot
            if (!temp.Tables.Contains("Shot"))
            {
                ShotTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [Shot] ADD CONSTRAINT PK_shot_id PRIMARY KEY (shot_id);";
                temp.ExecuteNonQuery(sql);
                // May need to add in the foreign keys?

                // Create the foreign key after the "ShotTable" has been created
                {
                    ShotTable = temp.Tables["Shot"]; // Retrieve the existing "ShotTable"

                    // User id                             Foriegn Key from Frame
                    var userIdKey = new ForeignKey(ShotTable, "FK_Shot_User");
                    var userIdKeyCol = new ForeignKeyColumn(userIdKey, "user_id")
                    {
                        ReferencedColumn = "id"
                    };
                    userIdKey.Columns.Add(userIdKeyCol);
                    userIdKey.ReferencedTable = "User";
                    userIdKey.Create();

                    // for testing purposes commenting out all the other keys

                    // Frame id                             Foriegn Key from Frame
                    var frameIdKey = new ForeignKey(ShotTable, "FK_Shot_Frame");
                    var frameIdKeyCol = new ForeignKeyColumn(frameIdKey, "frame_id")
                    {
                        ReferencedColumn = "frame_id"
                    };
                    frameIdKey.Columns.Add(frameIdKeyCol);
                    frameIdKey.ReferencedTable = "Frame";
                    frameIdKey.Create();

                    // Ball id                             Foriegn Key from Ball
                    var ballIdKey = new ForeignKey(ShotTable, "FK_Shot_Ball");
                    var ballIdKeyCol = new ForeignKeyColumn(ballIdKey, "ball_id")
                    {
                        ReferencedColumn = "ball_id"
                    };
                    ballIdKey.Columns.Add(ballIdKeyCol);
                    ballIdKey.ReferencedTable = "Ball";
                    ballIdKey.Create();

                    // Video id                             Foriegn Key from Video
                    var videoIdKey = new ForeignKey(ShotTable, "FK_Shot_Video");
                    var videoIdKeyCol = new ForeignKeyColumn(videoIdKey, "video_id")
                    {
                        ReferencedColumn = "video_id"
                    };
                    videoIdKey.Columns.Add(videoIdKeyCol);
                    videoIdKey.ReferencedTable = "Video";
                    videoIdKey.Create();
                }
            }
        }
    }
}