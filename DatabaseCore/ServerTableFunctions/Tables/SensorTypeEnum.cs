
using Microsoft.SqlServer.Management.Smo;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    /*
     * Create table SensorType (
	ID smallint,
	SensorType varchar(20),
    )

    Insert into SensorType(ID, SensorType) Values
    (1, 'LightSensor'),
    (2, 'Gyroscope'),
    (3, 'Accelerometer'),
    (4, 'Magnetometer');
    Original query */
    private void SensorTypeEnum(Database temp)
    {
        Console.WriteLine("Creating SensorType enum");
        // SensorType table
        var SensorTypeTable = new Table(temp, "SensorType");

        var id = new Column(SensorTypeTable, "id", DataType.SmallInt)
        {
            Nullable = false,
        };
        SensorTypeTable.Columns.Add(id);

        var SensorType = new Column(SensorTypeTable, "SensorType", DataType.VarChar(20))
        {
            Nullable = false,
        };
        SensorTypeTable.Columns.Add(SensorType);


        if (!temp.Tables.Contains("SensorType"))
        {
            SensorTypeTable.Create();

            // Insert values into enum
            string insertQuery = @"
            INSERT INTO SensorType (id, SensorType) VALUES 
            (1, 'LightSensor'),
            (2, 'Gyroscope'),
            (3, 'Accelerometer'),
            (4, 'Magnetometer');";
            temp.ExecuteNonQuery(insertQuery);

            Console.WriteLine("Success");
        }

    }
}
