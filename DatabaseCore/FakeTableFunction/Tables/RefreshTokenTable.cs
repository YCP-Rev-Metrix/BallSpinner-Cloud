using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{
    private void RefreshTokenTable(Database temp)
    {
        // RefreshToken Table
        {
            Console.WriteLine("Creating RefreshTokenTable and Token");
            // Create new
            var TokenTable = new Table(temp, "RefreshToken");

            // Expiration
            var expiration = new Column(TokenTable, "expiration", DataType.DateTime)
            {
                Nullable = false
            };
            TokenTable.Columns.Add(expiration);

            // User ID
            var userId = new Column(TokenTable, "userid", DataType.BigInt)
            {
                Nullable = false
            };
            TokenTable.Columns.Add(userId);

            // Token
            var token = new Column(TokenTable, "token", DataType.VarBinary(32))
            {
                Nullable = false
            };
            TokenTable.Columns.Add(token);

            if (!temp.Tables.Contains("RefreshToken"))
            {
                TokenTable.Create();

                TokenTable = temp.Tables["RefreshToken"];

                // User ID
                var userIdKey = new ForeignKey(TokenTable, "FK_RefreshToken_User");
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
    public void CreateDefaultToken()
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