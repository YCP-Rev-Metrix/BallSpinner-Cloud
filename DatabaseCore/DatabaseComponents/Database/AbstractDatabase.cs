using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;
public abstract class AbstractDatabase
{
    protected Server? Server;
    public Database? Database;
    protected string DatabaseName { get; set; }
    protected string? ConnectionString { get; set; }

    protected AbstractDatabase(string databaseName)
    {
        // process necessary environment variables
        string? dockerizedEnviron = Environment.GetEnvironmentVariable("DOCKERIZED");
        string? serverConnectionString = Environment.GetEnvironmentVariable("SERVER_CONNECTION_STRING");
        string? localConnectionString = Environment.GetEnvironmentVariable("LOCAL_CONNECTION_STRING");

        // create the connectionString used for connections to SQL Server, without direct connection to the DB (needed to create the DB)
        ConnectionString = dockerizedEnviron == null
            ? serverConnectionString
            // LocalConnectionString // uncomment this local connection string once local dev works again 
            : dockerizedEnviron == "Dockerized"
                ? serverConnectionString
                : localConnectionString;

        DatabaseName = databaseName;
        Initialize();
    }

    public void Initialize()
    {
        /*
        // This will also need to be edited to fix local development, not a huge issue at the moment
        serverConnection = new ServerConnection("localhost");
        Server = new Server(serverConnection);
        */

        Server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));
        Database = Server.Databases[DatabaseName];
    }
}
