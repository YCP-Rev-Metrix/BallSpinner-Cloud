using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void UserTable(Database temp)
    {
        // User Table
        {
            var UserTable = new Table(temp, "User");

            var id = new Column(UserTable, "id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            UserTable.Columns.Add(id);

            var firstname = new Column(UserTable, "firstname", DataType.VarChar(255))
            {
                Nullable = false
            };
            UserTable.Columns.Add(firstname);

            var lastname = new Column(UserTable, "lastname", DataType.VarChar(255))
            {
                Nullable = false
            };
            UserTable.Columns.Add(lastname);

            var username = new Column(UserTable, "username", DataType.VarChar(255))
            {
                Nullable = false
            };
            UserTable.Columns.Add(username);

            var password = new Column(UserTable, "password", DataType.VarBinary(-1))
            {
                Nullable = false
            };
            UserTable.Columns.Add(password);

            var salt = new Column(UserTable, "salt", DataType.VarBinary(16))
            {
                Nullable = false
            };
            UserTable.Columns.Add(salt);

            var email = new Column(UserTable, "email", DataType.VarChar(255));
            UserTable.Columns.Add(email);

            var phone = new Column(UserTable, "phone", DataType.VarChar(255));
            UserTable.Columns.Add(phone);

            var roles = new Column(UserTable, "roles", DataType.VarChar(255))
            {
                Nullable = false
            };
            UserTable.Columns.Add(roles);

            if (!temp.Tables.Contains("User"))
            {
                UserTable.Create();

                string sql = "ALTER TABLE [User] ADD CONSTRAINT User_PK PRIMARY KEY (id);";
                temp.ExecuteNonQuery(sql);

                sql = "ALTER TABLE [User] ADD CONSTRAINT Username_UNIQUE UNIQUE (username);";
                temp.ExecuteNonQuery(sql);
            }
        }
    }
}
