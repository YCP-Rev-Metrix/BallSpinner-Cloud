using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void SampleQueueIDTable(Database temp)
    {
        // SampleQueueID Table
        {
            var SampleQueueIDTable = new Table(temp, "SampleQueueID");

            var queue_id = new Column(SampleQueueIDTable, "queue_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            SampleQueueIDTable.Columns.Add(queue_id);

            var sensor_id = new Column(SampleQueueIDTable, "sensor_id", DataType.BigInt)
            {
                Nullable = true
            };
            SampleQueueIDTable.Columns.Add(sensor_id);

            var initial = new Column(SampleQueueIDTable, "initial", DataType.DateTime)
            {
                Nullable = true
            };
            SampleQueueIDTable.Columns.Add(initial);

            var final = new Column(SampleQueueIDTable, "final", DataType.DateTime)
            {
                Nullable = true
            };
            SampleQueueIDTable.Columns.Add(final);

            if (!temp.Tables.Contains("SampleQueueID"))
            {
                SampleQueueIDTable.Create();

                string sql = "ALTER TABLE [SampleQueueID] ADD CONSTRAINT SampleQueueID_PK PRIMARY KEY (queue_id);";
                temp.ExecuteNonQuery(sql);
            }
        }
    }

}