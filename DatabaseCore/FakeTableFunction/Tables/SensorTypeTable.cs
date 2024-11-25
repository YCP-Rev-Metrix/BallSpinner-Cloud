using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{
    private void SensorTypeTable(Database temp)
    {
        Console.WriteLine("Creating SensorTypeTable and temp data");
        var sensorType = new Table(temp, "SensorType");

        // sensor Id
        var typeid = new Column(sensorType, "type_id", DataType.BigInt)
        {
            Nullable = false
        };

        sensorType.Columns.Add(typeid);

        // type
        var type = new Column(sensorType, "type", DataType.VarChar(15))
        {
            Nullable = false
        };

        sensorType.Columns.Add(type);


        if (!temp.Tables.Contains("SensorType"))
        {
            sensorType.Create();
            
            string sql = "ALTER TABLE [SensorType] ADD CONSTRAINT Type_PK PRIMARY KEY (type_id);";
            temp.ExecuteNonQuery(sql);

            CreateDefaultSensorTypes();
            Console.WriteLine("Success");
        }
    }

    private void CreateDefaultSensorTypes()
    {
        string[] sensorNames = { "Lightsensor", "Accelerometer", "Gyroscope", "Magnetometer" };

        string sql = "INSERT INTO [SensorType] (type_id, type) " +
                     "VALUES (@typeid, @type);";
        int i = 1;
        
        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            foreach (string sensorName in sensorNames)
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@typeid", i);
                    cmd.Parameters.AddWithValue("@type", sensorName);
                    
                    // Execute the query
                    cmd.ExecuteNonQuery();
                    i++;
                }
            }
            
        }
    }
}