using Common.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public abstract class AbstractDatabase
{
    protected Server Server;
    public Database database;
    public string DatabaseName { get; set; }
    public string ConnectionString { get; set; }

    public AbstractDatabase(string databaseName)
    {
        // process necessary environment variables
        string? DockerizedEnviron = Environment.GetEnvironmentVariable("DOCKERIZED");
        string? ServerConnectionString = Environment.GetEnvironmentVariable("SERVER_CONNECTION_STRING");
        string? LocalConnectionString = Environment.GetEnvironmentVariable("LOCAL_CONNECTION_STRING");

        // create the connectionString used for connections to SQL Server, without direct connection to the DB (needed to create the DB)
        ConnectionString = DockerizedEnviron == null
            ? ServerConnectionString
            // LocalConnectionString // uncomment this local connection string once local dev works again 
            : DockerizedEnviron == "Dockerized"
                ? ServerConnectionString
                : LocalConnectionString;

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
        database = Server.Databases[DatabaseName];
    }
}
