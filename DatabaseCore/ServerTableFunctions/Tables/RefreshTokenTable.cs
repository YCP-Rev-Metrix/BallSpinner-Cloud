using Microsoft.SqlServer.Management.Smo;

using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    private void RefreshTokenTable(Database temp)
    {
        // RefreshToken Table
        {
            Console.WriteLine("Creating RefreshTokenTable");
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
                Console.WriteLine("Success");
            }

        }
    }
}