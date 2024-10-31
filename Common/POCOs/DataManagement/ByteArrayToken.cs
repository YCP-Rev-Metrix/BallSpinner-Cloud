namespace Common.POCOs;

/// <summary>
/// Defines a POCO that is a byte[] representing a Token
/// </summary>
public class ByteArrayToken : Poco
{
    public ByteArrayToken(byte[] token) => Token = token;

    public byte[] Token { get; set; }
}
