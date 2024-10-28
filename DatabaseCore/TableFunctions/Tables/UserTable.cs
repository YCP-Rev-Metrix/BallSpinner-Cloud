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
            // Create new
            var UserTable = new Table(temp, "User");

            // Firstname
            var firstname = new Column(UserTable, "firstname", DataType.VarChar(255))
            {
                Nullable = false
            };
            UserTable.Columns.Add(firstname);

            // Lastname
            var lastname = new Column(UserTable, "lastname", DataType.VarChar(255))
            {
                Nullable = false
            };
            UserTable.Columns.Add(lastname);

            // Username
            var username = new Column(UserTable, "username", DataType.VarChar(255))
            {
                Nullable = false
            };
            UserTable.Columns.Add(username);

            // Salt
            var salt = new Column(UserTable, "salt", DataType.VarBinary(16))
            {
                Nullable = false
            };
            UserTable.Columns.Add(salt);

            // Roles
            var roles = new Column(UserTable, "roles", DataType.VarChar(255))
            {
                Nullable = false
            };
            UserTable.Columns.Add(roles);

            // Password
            var password = new Column(UserTable, "password", DataType.VarBinaryMax)
            {
                Nullable = false
            };
            UserTable.Columns.Add(password);

            // Email
            var email = new Column(UserTable, "email", DataType.VarChar(255));
            UserTable.Columns.Add(email);

            // Phone
            var phone = new Column(UserTable, "phone", DataType.VarChar(255));
            UserTable.Columns.Add(phone);

            // ID
            var id = new Column(UserTable, "id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            UserTable.Columns.Add(id);

            if (!temp.Tables.Contains("User"))
            {
                UserTable.Create();

                // Create the primary key constraint using SQL
                string sql = "ALTER TABLE [User] ADD CONSTRAINT PK_User PRIMARY KEY (id);";

                temp.ExecuteNonQuery(sql);

                sql = "ALTER TABLE [User] ADD CONSTRAINT UNQ__User__username UNIQUE ([username])";
                temp.ExecuteNonQuery(sql);
            }
        }
    }
}