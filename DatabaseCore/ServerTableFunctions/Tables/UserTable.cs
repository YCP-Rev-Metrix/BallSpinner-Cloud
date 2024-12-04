
using Microsoft.SqlServer.Management.Smo;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    private void UserTable(Database temp)
    {
        Console.WriteLine("Creating UserTable");
        // User Table
        var userTable = new Table(temp, "User");

        var id = new Column(userTable, "id", DataType.BigInt)
        {
            IdentityIncrement = 1,
            Nullable = false,
            IdentitySeed = 1,
            Identity = true
        };
        userTable.Columns.Add(id);

        var firstname = new Column(userTable, "firstname", DataType.VarChar(50))
        {
            Nullable = true
        };
        userTable.Columns.Add(firstname);

        var lastname = new Column(userTable, "lastname", DataType.VarChar(50))
        {
            Nullable = true
        };
        userTable.Columns.Add(lastname);

        var username = new Column(userTable, "username", DataType.VarChar(50))
        {
            Nullable = false
        };
        userTable.Columns.Add(username);

        var password = new Column(userTable, "password", DataType.VarBinary(64))
        {
            Nullable = false
        };
        userTable.Columns.Add(password);

        var salt = new Column(userTable, "salt", DataType.VarBinary(64))
        {
            Nullable = false
        };
        userTable.Columns.Add(salt);

        var email = new Column(userTable, "email", DataType.VarChar(50));
        userTable.Columns.Add(email);

        var phone = new Column(userTable, "phone", DataType.VarChar(50));
        userTable.Columns.Add(phone);

        var roles = new Column(userTable, "roles", DataType.VarChar(50))
        {
            Nullable = false
        };
        userTable.Columns.Add(roles);

        if (!temp.Tables.Contains("User"))
        {
            userTable.Create();

            string sql = "ALTER TABLE [User] ADD CONSTRAINT User_PK PRIMARY KEY (id);";
            temp.ExecuteNonQuery(sql);

            sql = "ALTER TABLE [User] ADD CONSTRAINT Username_UNIQUE UNIQUE (username);";
            temp.ExecuteNonQuery(sql);

            Console.WriteLine("Success");
        }
    }
}
