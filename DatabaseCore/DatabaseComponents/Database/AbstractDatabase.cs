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
        string? localConnectionString = Environment.GetEnvironmentVariable("LOCAL_CONNECTION_STRING");
        string? localDBConnectionString = Environment.GetEnvironmentVariable("LOCALDB_CONNECTION_STRING");
        if (dockerizedEnviron == null)
        {
            Environment.SetEnvironmentVariable("SERVER_CONNECTION_STRING", localConnectionString);
            Environment.SetEnvironmentVariable("SERVERDB_CONNECTION_STRING", localDBConnectionString);
        }

        ConnectionString = Environment.GetEnvironmentVariable("SERVER_CONNECTION_STRING");

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
public static class SqlDataReaderExtensions
{
    public static T? GetNullableValue<T>(this SqlDataReader reader, string columnName) where T : struct
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? (T?)null : reader.GetFieldValue<T>(ordinal);
    }
}
