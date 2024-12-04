using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{
    private void SensorTypeEnum(Database temp)
    {
        Console.WriteLine("Creating SensorType enum");
        // SensorType table
        var SensorTypeTable = new Table(temp, "SensorType");

        var id = new Column(SensorTypeTable, "type_id", DataType.SmallInt)
        {
            Nullable = false,
        };
        SensorTypeTable.Columns.Add(id);

        var SensorType = new Column(SensorTypeTable, "type", DataType.VarChar(20))
        {
            Nullable = false,
        };
        SensorTypeTable.Columns.Add(SensorType);


        if (!temp.Tables.Contains("SensorType"))
        {
            SensorTypeTable.Create();

            string sql = "ALTER TABLE [SensorType] ADD CONSTRAINT typeid_PK PRIMARY KEY (type_id);";
            temp.ExecuteNonQuery(sql);

            // Insert values into enum
            string insertQuery = @"
            INSERT INTO SensorType (type_id, type) VALUES 
            (1, 'LightSensor'),
            (2, 'Gyroscope'),
            (3, 'Accelerometer'),
            (4, 'Magnetometer');";
            temp.ExecuteNonQuery(insertQuery);

            Console.WriteLine("Success");
        }

    }
}