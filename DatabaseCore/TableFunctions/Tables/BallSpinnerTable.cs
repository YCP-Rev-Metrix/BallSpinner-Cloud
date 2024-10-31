using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void BallSpinnerTable(Database temp)
    {
        // BallSpinner Table
        {
            var BallSpinnerTable = new Table(temp, "BallSpinner");

            var ballspinner_id = new Column(BallSpinnerTable, "ballspinner_id", DataType.BigInt)
            {
                Nullable = false
            };
            BallSpinnerTable.Columns.Add(ballspinner_id);

            var mac_address = new Column(BallSpinnerTable, "mac_address", DataType.VarChar(48))
            {
                Nullable = false
            };
            BallSpinnerTable.Columns.Add(mac_address);

            var name = new Column(BallSpinnerTable, "name", DataType.VarChar(100))
            {
                Nullable = false
            };
            BallSpinnerTable.Columns.Add(name);

            if (!temp.Tables.Contains("BallSpinner"))
            {
                BallSpinnerTable.Create();

                string sql = "ALTER TABLE [BallSpinner] ADD CONSTRAINT BallSpinner_PK PRIMARY KEY (ballspinner_id);";
                temp.ExecuteNonQuery(sql);
            }
        }
    }
}