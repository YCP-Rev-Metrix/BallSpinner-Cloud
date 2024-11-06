using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void BallTable(Database temp)
    {
        // Ball Table
        {
            // Create new table
            var BallTable = new Table(temp, "Ball");

            // ball_id
            var ball_id = new Column(BallTable, "ball_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            BallTable.Columns.Add(ball_id);

            // weight
            var weight = new Column(BallTable, "weight", DataType.Float)
            {
                Nullable = false
            };
            BallTable.Columns.Add(weight);

            // color
            var color = new Column(BallTable, "color", DataType.VarChar(50));
            BallTable.Columns.Add(color);

            if (!temp.Tables.Contains("Ball"))
            {
                BallTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [Ball] ADD CONSTRAINT PK_Ball PRIMARY KEY (ball_id);";

                temp.ExecuteNonQuery(sql);
            }
        }
    }
}