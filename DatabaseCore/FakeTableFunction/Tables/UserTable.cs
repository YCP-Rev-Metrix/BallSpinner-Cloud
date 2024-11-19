
using Microsoft.SqlServer.Management.Smo;
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
    private (byte[] hashed, byte[] salt) SaltHashPassword(string? password)
    {
        byte[] salt = GenerateRandomBytes(16);
        byte[] hashed = SaltHashPassword(password, salt);
        return (hashed, salt);
    }
    private byte[] SaltHashPassword(string? password, byte[] salt)
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

    private void CreateDefaultUser()
    {
        (byte[] hashedPass, byte[] saltPass) = SaltHashPassword("string");

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
                cmd.Parameters.AddWithValue("@Salt", saltPass);  
                cmd.Parameters.AddWithValue("@Roles", "admin");
                cmd.Parameters.AddWithValue("@Password", hashedPass);  
                cmd.Parameters.AddWithValue("@Email", "string@example.com");
                cmd.Parameters.AddWithValue("@Phone", "string");

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
}
