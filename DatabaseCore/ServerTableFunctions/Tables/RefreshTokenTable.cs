using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void RefreshTokenTable(Database temp)
    {
        // RefreshToken Table
        {
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

                // Create the foreign key after the "RefreshTokenTable" has been created
                {
                    TokenTable = temp.Tables["RefreshToken"]; // Retrieve the existing "RefreshTokenTable"

                    // User ID
                    var userIdKey = new ForeignKey(TokenTable, "FK_RefreshToken_User");
                    var userIdKeyCol = new ForeignKeyColumn(userIdKey, "userid")
                    {
                        ReferencedColumn = "id"
                    };
                    userIdKey.Columns.Add(userIdKeyCol);
                    userIdKey.ReferencedTable = "User";

                    userIdKey.Create();
                }
            }
        }
    }
}