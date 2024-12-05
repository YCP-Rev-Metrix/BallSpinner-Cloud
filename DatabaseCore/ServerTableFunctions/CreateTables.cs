using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class Dbcore
{
    public void CreateTables()
    {
        var database = new Microsoft.SqlServer.Management.Smo.Database(Server, DatabaseName);

        // will need to look at this part
        if (!Server.Databases.Contains(DatabaseName))
        {
            database.Create();
        }
        Database temp = Server.Databases[DatabaseName];

        // call each function to create the respective tables
        SensorTypeEnum(temp);
        UserTable(temp);
        RefreshTokenTable(temp);
        BallTable(temp);
        ArsenalTable(temp);
        SmartDotTable(temp);
        SmartDotListTable(temp);
        //BallSpinnerTable(temp);
        //BallSpinnerList(temp);
        SimulatedShotTable(temp);
        SimulatedShotListTable(temp);
        //BallSpinnerSensorsTable(temp);
        //BS_SensorsTable(temp);
        SDSensorTable(temp);
        SensorDataTable(temp);
        UserLocalShotsTable(temp);


    }
}