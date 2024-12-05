using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class Dbcoretest
{
    private void SDSensorTable(Database temp)
    {
        Console.WriteLine("Creating SDSensorTable and temp data");
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
            var SDIdKey = new ForeignKey(sdSensor, "SD_Sensor_SimulatedShot_FK");
            var SDIdKeyCol = new ForeignKeyColumn(SDIdKey, "shotid")
            {
                ReferencedColumn = "shotid"
            };
            SDIdKey.Columns.Add(SDIdKeyCol);
            SDIdKey.ReferencedTable = "SimulatedShot";

            SDIdKey.Create();
            
            // SensorType - SDSensor FK
            var TypeIdKey = new ForeignKey(sdSensor, "SD_Sensor_SensorType_FK");
            var TypeIdKeyCol = new ForeignKeyColumn(TypeIdKey, "type_id")
            {
                ReferencedColumn = "type_id"
            };
            TypeIdKey.Columns.Add(TypeIdKeyCol);
            TypeIdKey.ReferencedTable = "SensorType";

            TypeIdKey.Create();
            
            CreateDefaultSmartDotSensor();

            Console.WriteLine("Success");
        }
    }

    private void CreateDefaultSmartDotSensor()
    {
        string sql = "INSERT INTO [SD_Sensor] (frequency, type_id, shotid) " +
                     "VALUES (@frequency, @typeid, @shotid)";

        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@frequency", 15);
                cmd.Parameters.AddWithValue("@typeid", 1);
                cmd.Parameters.AddWithValue("@shotid", "1");


                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
}