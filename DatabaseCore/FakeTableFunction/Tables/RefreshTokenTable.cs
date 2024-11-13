using Microsoft.SqlServer.Management.Smo;

using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{
    private void RefreshTokenTable(Database temp)
    {
        // RefreshToken Table
        {
            Console.WriteLine("Creating RefreshTokenTable and temp data");
            // Create new
            var tokenTable = new Table(temp, "RefreshToken");

            // Expiration
            var expiration = new Column(tokenTable, "expiration", DataType.DateTime)
            {
                Nullable = false
            };
            tokenTable.Columns.Add(expiration);

            // User ID
            var userId = new Column(tokenTable, "userid", DataType.BigInt)
            {
                Nullable = false
            };
            tokenTable.Columns.Add(userId);

            // Token
            var token = new Column(tokenTable, "token", DataType.VarBinary(32))
            {
                Nullable = false
            };
            tokenTable.Columns.Add(token);

            if (!temp.Tables.Contains("RefreshToken"))
            {
                tokenTable.Create();

                tokenTable = temp.Tables["RefreshToken"];

                // User ID
                var userIdKey = new ForeignKey(tokenTable, "FK_RefreshToken_User");
                var userIdKeyCol = new ForeignKeyColumn(userIdKey, "userid")
                {
                    ReferencedColumn = "id"
                };
                userIdKey.Columns.Add(userIdKeyCol);
                userIdKey.ReferencedTable = "User";

                userIdKey.Create();
                CreateDefaultToken();
                Console.WriteLine("Success");
            }
            
        }
    }
    private void CreateDefaultToken()
    {
        string sql = "INSERT INTO [RefreshToken] (expiration, userid, token) " +
                     "VALUES (@Expiration, @Userid, (CONVERT(varbinary(32), @Token)))";
        
        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@Expiration", "2024-11-06 19:01:24.813");
                cmd.Parameters.AddWithValue("@Userid", "1");
                cmd.Parameters.AddWithValue("@Token", "testusertoken");

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
}