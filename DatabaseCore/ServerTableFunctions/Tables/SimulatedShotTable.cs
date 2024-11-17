using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void SimulatedShotTable(Database temp)
    {
        Console.WriteLine("Creating SimulatedShotTable and temp data");
        var simulatedShot = new Table(temp, "SimulatedShot");
        
        var shotid = new Column(simulatedShot, "shotid", DataType.BigInt)
        {
            IdentityIncrement = 1,
            Nullable = false,
            IdentitySeed = 1,
            Identity = true
        };
        simulatedShot.Columns.Add(shotid);
    
        var speed = new Column(simulatedShot, "speed", DataType.Float)
        {
            Nullable = false
        };
        simulatedShot.Columns.Add(speed);
    
        var shotAngle = new Column(simulatedShot, "angle", DataType.Float)
        {
            Nullable = true
        };
        simulatedShot.Columns.Add(shotAngle);
    
        var position = new Column(simulatedShot, "position", DataType.Float)
        {
            Nullable = true
        };
        simulatedShot.Columns.Add(position);
    
        var smartDotSensorsId = new Column(simulatedShot, "smartdot_sensorsid", DataType.BigInt)
        {
            Nullable = true
        };
        simulatedShot.Columns.Add(smartDotSensorsId);
    
        var ballSpinnerSensorsId = new Column(simulatedShot, "ballspinner_sensorsid", DataType.BigInt)
        {
            Nullable = true
        };
        simulatedShot.Columns.Add(ballSpinnerSensorsId);
    
        var name = new Column(simulatedShot, "name", DataType.VarChar(30))
        {
            Nullable = false
        };
        simulatedShot.Columns.Add(name);
        
        var created = new Column(simulatedShot, "Created", DataType.DateTime)
        {
            Nullable = true
        };
        simulatedShot.Columns.Add(created);
        
        if (!temp.Tables.Contains("SimulatedShot"))
        {
            simulatedShot.Create();
            
            simulatedShot = temp.Tables["SimulatedShot"];
            
            
            string sql = "ALTER TABLE [SimulatedShot] ADD CONSTRAINT SimulatedShot_PK PRIMARY KEY (shotid);";
            temp.ExecuteNonQuery(sql);

            sql = "ALTER TABLE [SimulatedShot] ADD CONSTRAINT ShotName_UNIQUE UNIQUE (name);";
            temp.ExecuteNonQuery(sql);

            Console.WriteLine("Success");
        }
    }
}