using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void VideoTable(Database temp)
    {
        // Video Table
        {
            // Create new table
            var VideoTable = new Table(temp, "Video");

            // practice_id
            var video_id = new Column(VideoTable, "video_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            VideoTable.Columns.Add(video_id);

            // Event Id
            var video = new Column(VideoTable, "video", DataType.VarBinaryMax)
            {
                Nullable = false
            };
            VideoTable.Columns.Add(video);

            if (!temp.Tables.Contains("Video"))
            {
                VideoTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [Video] ADD CONSTRAINT PK_Video PRIMARY KEY (video_id);";

                temp.ExecuteNonQuery(sql);
            }
        }
    }
}