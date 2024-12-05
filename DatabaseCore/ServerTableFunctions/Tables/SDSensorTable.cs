using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class Dbcore
{
    private void SDSensorTable(Database temp)
    {
        Console.WriteLine("Creating SDSensorTable");
        var sdSensor = new Table(temp, "SD_Sensor");

        var sensorid = new Column(sdSensor, "sensor_id", DataType.BigInt)
        {
            IdentityIncrement = 1,
            Nullable = false,
            IdentitySeed = 1,
            Identity = true
        };
        sdSensor.Columns.Add(sensorid);

        var shotid = new Column(sdSensor, "shotid", DataType.BigInt)
        {
            Nullable = false
        };
        sdSensor.Columns.Add(shotid);

        var frequency = new Column(sdSensor, "frequency", DataType.Float)
        {
            Nullable = false
        };
        sdSensor.Columns.Add(frequency);
        
        var type = new Column(sdSensor, "type_id", DataType.SmallInt)
        {
            Nullable = false
        };
        sdSensor.Columns.Add(type);

        if (!temp.Tables.Contains("SD_Sensor"))
        {
            sdSensor.Create();

            string sql = "ALTER TABLE [SD_Sensor] ADD CONSTRAINT SmartDotSensor_PK PRIMARY KEY (sensor_id);";
            temp.ExecuteNonQuery(sql);

            
            // SimulatedShot - SDSensor FK
            var sdIdKey = new ForeignKey(sdSensor, "SD_Sensor_SimulatedShot_FK");
            var sdIdKeyCol = new ForeignKeyColumn(sdIdKey, "shotid")
            {
                ReferencedColumn = "shotid"
            };
            sdIdKey.Columns.Add(sdIdKeyCol);
            sdIdKey.ReferencedTable = "SimulatedShot";

            sdIdKey.DeleteAction = ForeignKeyAction.Cascade;

            sdIdKey.Create();
            
            // SensorType - SDSensor FK
            var typeIdKey = new ForeignKey(sdSensor, "SD_Sensor_SensorType_FK");
            var typeIdKeyCol = new ForeignKeyColumn(typeIdKey, "type_id")
            {
                ReferencedColumn = "type_id"
            };
            typeIdKey.Columns.Add(typeIdKeyCol);
            typeIdKey.ReferencedTable = "SensorType";

            typeIdKey.DeleteAction = ForeignKeyAction.NoAction;
            typeIdKey.Create();
            
            Console.WriteLine("Success");
        }
    }
}