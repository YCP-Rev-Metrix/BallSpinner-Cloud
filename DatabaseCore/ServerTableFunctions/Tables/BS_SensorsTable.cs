using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void BS_SensorsTable(Database temp)
    {
        // BS_sensors Table
        {
            var BS_SensorsTable = new Table(temp, "BS_sensors");

            var sensorid = new Column(BS_SensorsTable, "sensorid", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            BS_SensorsTable.Columns.Add(sensorid);

            var rpm = new Column(BS_SensorsTable, "rpm", DataType.Decimal(38, 0))
            {
                Nullable = true
            };
            BS_SensorsTable.Columns.Add(rpm);

            var datetime = new Column(BS_SensorsTable, "datetime", DataType.DateTime)
            {
                Nullable = true
            };
            BS_SensorsTable.Columns.Add(datetime);

            if (!temp.Tables.Contains("BS_sensors"))
            {
                BS_SensorsTable.Create();

                string sql = "ALTER TABLE [BS_sensors] ADD CONSTRAINT BS_sensors_PK PRIMARY KEY (sensorid);";
                temp.ExecuteNonQuery(sql);
            }
        }
    }
}