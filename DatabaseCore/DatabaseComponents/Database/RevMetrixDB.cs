using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB : AbstractDatabase
{
    public RevMetrixDB() : base(getDatabaseName()) { }

    public static string getDatabaseName()
    {
        string env = Environment.GetEnvironmentVariable("TESTDB_ENV");
        if (env == "true")
        {
            return "revmetrix-test";
        }
        else
        {
            return "revmetrix-bs";
        }
    }
} 
public partial class RevMetrixBSTest : AbstractDatabase
{
    public RevMetrixBSTest() : base("revmetrix-test") { }
} 