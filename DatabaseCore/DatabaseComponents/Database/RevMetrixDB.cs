﻿using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb : AbstractDatabase
{
    public RevMetrixDb() : base(getDatabaseName()) { }

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
public partial class Dbcoretest : AbstractDatabase
{
    public Dbcoretest() : base("revmetrix-test") { }
} 
public partial class Dbcore : AbstractDatabase
{
    public Dbcore() : base("revmetrix-bs") { }
}