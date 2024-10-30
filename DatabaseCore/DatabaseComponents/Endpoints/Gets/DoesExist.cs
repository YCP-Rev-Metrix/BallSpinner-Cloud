using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public bool DoesExist()
    {
        Database = new Microsoft.SqlServer.Management.Smo.Database(Server, DatabaseName);
        return !Server.Databases.Contains(DatabaseName);
    }
}