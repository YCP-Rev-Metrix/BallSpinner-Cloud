using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB : AbstractDatabase
{
    public RevMetrixDB() : base("revmetrix-db") { }
} 
