using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
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
        UserTable(temp);
        RefreshTokenTable(temp);
        EventTable(temp);
        LeagueTable(temp); 
        TournamentTable(temp);
        PracticeTable(temp);
        SessionTable(temp);
        GameTable(temp);
        FrameTable(temp);
        VideoTable(temp);
        BallTable(temp);
        ShotTable(temp);
    }
}