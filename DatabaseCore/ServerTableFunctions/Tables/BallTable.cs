using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    private void BallTable(Database temp)
    {
        Console.WriteLine("Creating BallTable");
        var ballTable = new Table(temp, "Ball");

        // Ball Id
        var ballid = new Column(ballTable, "ballid", DataType.BigInt)
        {
            IdentityIncrement = 1,
            Nullable = false,
            IdentitySeed = 1,
            Identity = true
        };

        ballTable.Columns.Add(ballid);

        var name = new Column(ballTable, "name", DataType.VarChar(100))
        {
            Nullable = false
        };

        ballTable.Columns.Add(name);

        var weight = new Column(ballTable, "weight", DataType.Float)
        {
            Nullable = false
        };

        ballTable.Columns.Add(weight);

        var hardness = new Column(ballTable, "hardness", DataType.Float)
        {
            Nullable = true
        };

        ballTable.Columns.Add(hardness);

        var coretype = new Column(ballTable, "core_type", DataType.VarChar(25))
        {
            Nullable = true
        };

        ballTable.Columns.Add(coretype);

        if (!temp.Tables.Contains("Ball"))
        {
            ballTable.Create();

            string sql = "ALTER TABLE [Ball] ADD CONSTRAINT Ball_PK PRIMARY KEY (ballid);";
            temp.ExecuteNonQuery(sql);
            sql = "ALTER TABLE [Ball] ADD CONSTRAINT BallName_UNIQUE UNIQUE (name);";
            temp.ExecuteNonQuery(sql);
            Console.WriteLine("Success");
        }
    }
}