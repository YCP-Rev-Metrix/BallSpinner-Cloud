namespace Common.POCOs;

/// <summary>
/// Defines a POCO representing a salted and hashed password
/// </summary>
public class HashAndSalt : Poco
{
    public byte[] Hash { set; get; }
    public byte[] Salt { set; get; }

    public HashAndSalt(byte[] hash, byte[] salt)
    {
        Hash = hash;
        Salt = salt;
    }
}
