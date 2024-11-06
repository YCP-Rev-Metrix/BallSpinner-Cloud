using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixBSTest
{
    private void UserTable(Database temp)
{
    Console.WriteLine("Creating UserTable and User");
    // User Table
    var UserTable = new Table(temp, "User");

    var id = new Column(UserTable, "id", DataType.BigInt)
    {
        IdentityIncrement = 1,
        Nullable = false,
        IdentitySeed = 1,
        Identity = true
    };
    UserTable.Columns.Add(id);

    var firstname = new Column(UserTable, "firstname", DataType.VarChar(50))
    {
        Nullable = false
    };
    UserTable.Columns.Add(firstname);

    var lastname = new Column(UserTable, "lastname", DataType.VarChar(50))
    {
        Nullable = false
    };
    UserTable.Columns.Add(lastname);

    var username = new Column(UserTable, "username", DataType.VarChar(50))
    {
        Nullable = false
    };
    UserTable.Columns.Add(username);

    var password = new Column(UserTable, "password", DataType.VarBinary(64))
    {
        Nullable = false
    };
    UserTable.Columns.Add(password);

    var salt = new Column(UserTable, "salt", DataType.VarBinary(64))
    {
        Nullable = false
    };
    UserTable.Columns.Add(salt);

    var email = new Column(UserTable, "email", DataType.VarChar(50));
    UserTable.Columns.Add(email);

    var phone = new Column(UserTable, "phone", DataType.VarChar(50));
    UserTable.Columns.Add(phone);

    var roles = new Column(UserTable, "roles", DataType.VarChar(50))
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

        CreateDefaultUser();
        Console.WriteLine("Success");
    }
}

    public byte[] GenerateRandomBytes(int length)
    {
        byte[] randomBytes = new byte[length];
        _randomNumberGenerator.GetBytes(randomBytes);
        return randomBytes;
    }
    public (byte[] hashed, byte[] salt) SaltHashPassword(string? password)
    {
        byte[] salt = GenerateRandomBytes(16);
        byte[] hashed = SaltHashPassword(password, salt);
        return (hashed, salt);
    }
    public byte[] SaltHashPassword(string? password, byte[] salt)
    {
        byte[] passwordbytes = Encoding.ASCII.GetBytes(password);
        var s = new MemoryStream();
        s.Write(passwordbytes, 0, passwordbytes.Length);
        s.Write(salt, 0, salt.Length);
        byte[] combined = s.ToArray();
        byte[] hashed = SHA256.HashData(combined);
        return hashed;
    }    
    
    private readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

    public void CreateDefaultUser()
    {
        (byte[] hashed_pass, byte[] salt_pass) = SaltHashPassword("your_password");

        string sql = "INSERT INTO [User] (firstname, lastname, username, salt, roles, password, email, phone) " +
                     "VALUES (@FirstName, @LastName, @Username, @Salt, @Roles, @Password, @Email, @Phone)";
        
        string? serverConnectionString = Environment.GetEnvironmentVariable("TESTBS_CONNECTION_STRING");

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@FirstName", "string");
                cmd.Parameters.AddWithValue("@LastName", "string");
                cmd.Parameters.AddWithValue("@Username", "string");
                cmd.Parameters.AddWithValue("@Salt", salt_pass);  
                cmd.Parameters.AddWithValue("@Roles", "user");
                cmd.Parameters.AddWithValue("@Password", hashed_pass);  
                cmd.Parameters.AddWithValue("@Email", "string@example.com");
                cmd.Parameters.AddWithValue("@Phone", "string");

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
}
