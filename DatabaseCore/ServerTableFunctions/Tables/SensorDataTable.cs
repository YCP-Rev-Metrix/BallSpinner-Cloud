using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    private void SensorDataTable(Database temp)
    {
        Console.WriteLine("Creating SampleDataTable");
        
        var sampleData = new Table(temp, "SensorData");

        // Create all neccesarry Columns
        var sensorId = new Column(sampleData, "sensor_id", DataType.BigInt)
        {
            Nullable = false
        };
        sampleData.Columns.Add(sensorId);

        var count = new Column(sampleData, "count", DataType.Int)
        {
            Nullable = true
        };
        sampleData.Columns.Add(count);

        var brightness = new Column(sampleData, "brightness", DataType.Float)
        {
            Nullable = true
        };
        sampleData.Columns.Add(brightness);
        
        var xaxis = new Column(sampleData, "xaxis", DataType.Float)
        {
            Nullable = true
        };
        sampleData.Columns.Add(xaxis);

        
        var yaxis = new Column(sampleData, "yaxis", DataType.Float)
        {
            Nullable = true
        };
        sampleData.Columns.Add(yaxis);
        
        var zaxis = new Column(sampleData, "zaxis", DataType.Float)
        {
            Nullable = true
        };
        sampleData.Columns.Add(zaxis);
        
        var waxis = new Column(sampleData, "waxis", DataType.Float)
        {
            Nullable = true
        };
        sampleData.Columns.Add(waxis);
        
        var logtime = new Column(sampleData, "logtime", DataType.Float)
        {
            Nullable = true
        };
        sampleData.Columns.Add(logtime);
        
        
        
        if (!temp.Tables.Contains("SensorData"))
        {
            sampleData.Create();
            
            // Foreign key for Sensor ID
            var sensorIdKey = new ForeignKey(sampleData, "SampleData_SDSensor_FK");
            
            var sensorIdKeyCol = new ForeignKeyColumn(sensorIdKey, "sensor_id")
            {
                ReferencedColumn = "sensor_id"
            };
            
            sensorIdKey.Columns.Add(sensorIdKeyCol);
            
            sensorIdKey.ReferencedTable = "SD_Sensor";

            sensorIdKey.Create();
            
            Console.WriteLine("Success");
        }
    }
}