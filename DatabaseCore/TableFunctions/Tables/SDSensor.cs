using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void SDSensorTable(Database temp)
    {
        // SMSensor Table
        {
            var SDSensorTable = new Table(temp, "SD_Sensor");

            var sensorid = new Column(SDSensorTable, "sensorid", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            SDSensorTable.Columns.Add(sensorid);

            var samples_in_queue = new Column(SDSensorTable, "samples_in_queue", DataType.Int)
            {
                Nullable = true
            };
            SDSensorTable.Columns.Add(samples_in_queue);

            var sample_frequency = new Column(SDSensorTable, "sample_frequency", DataType.Int)
            {
                Nullable = true
            };
            SDSensorTable.Columns.Add(sample_frequency);

            if (!temp.Tables.Contains("SDSensor"))
            {
                SDSensorTable.Create();

                string sql = "ALTER TABLE [SDSensor] ADD CONSTRAINT SmartDotSensor_PK PRIMARY KEY (sensorid);";
                temp.ExecuteNonQuery(sql);
            }
        }
    }
}