using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
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
    
        var ballid = new Column(simulatedShot, "ballid", DataType.BigInt)
        {
            Nullable = true
        };
        simulatedShot.Columns.Add(ballid);
    
        var initialSpeed = new Column(simulatedShot, "initial_speed", DataType.Float)
        {
            Nullable = true
        };
        simulatedShot.Columns.Add(initialSpeed);
    
        var shotAngle = new Column(simulatedShot, "shot_angle", DataType.Float)
        {
            Nullable = false
        };
        simulatedShot.Columns.Add(shotAngle);
    
        var shotPosition = new Column(simulatedShot, "shot_position", DataType.Float)
        {
            Nullable = false
        };
        simulatedShot.Columns.Add(shotPosition);
    
        var smartDotSensorsId = new Column(simulatedShot, "smartdot_sensorsid", DataType.BigInt)
        {
            Nullable = false
        };
        simulatedShot.Columns.Add(smartDotSensorsId);
    
        var ballSpinnerSensorsId = new Column(simulatedShot, "ballspinner_sensorsid", DataType.BigInt)
        {
            Nullable = false
        };
        simulatedShot.Columns.Add(ballSpinnerSensorsId);
    
        var name = new Column(simulatedShot, "name", DataType.VarChar(30))
        {
            Nullable = true
        };
        simulatedShot.Columns.Add(name);
        if (!temp.Tables.Contains("SimulatedShot"))
        {
            simulatedShot.Create();
            
            simulatedShot = temp.Tables["SimulatedShot"];

            // Foreign key for Ballid
            var ballIdKey = new ForeignKey(simulatedShot, "SimulatedShot_Ball_FK");
            var ballIdKeyCol = new ForeignKeyColumn(ballIdKey, "ballid")
            {
                ReferencedColumn = "ballid"
            };
            ballIdKey.Columns.Add(ballIdKeyCol);
            ballIdKey.ReferencedTable = "Ball";

            ballIdKey.Create();
            
            string sql = "ALTER TABLE [SimulatedShot] ADD CONSTRAINT SimulatedShot_PK PRIMARY KEY (shotid);";
            temp.ExecuteNonQuery(sql);

            sql = "ALTER TABLE [SimulatedShot] ADD CONSTRAINT ShotName_UNIQUE UNIQUE (name);";
            temp.ExecuteNonQuery(sql);

            CreateDefaultSimulatedShot();
            Console.WriteLine("Success");
        }
    }

    private void CreateDefaultSimulatedShot()
    {

        string sql = "INSERT INTO [SimulatedShot] (ballid, initial_speed, shot_angle, shot_position, smartdot_sensorsid, ballspinner_sensorsid, name) " +
                     "VALUES (@Ballid, @Initial_speed, @Shot_angle, @Shot_position, @Smartdot_sensorsid, @Ballspinner_sensorsid, @Name)";
        
        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@Ballid", "1");
                cmd.Parameters.AddWithValue("@Initial_speed", 3.2);
                cmd.Parameters.AddWithValue("@Shot_angle", 1.2);
                cmd.Parameters.AddWithValue("@Shot_position", 7.4);  
                cmd.Parameters.AddWithValue("@Smartdot_sensorsid", "1");
                cmd.Parameters.AddWithValue("@Ballspinner_sensorsid", "1");  
                cmd.Parameters.AddWithValue("@Name", "BestShot");

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
}