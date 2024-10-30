namespace Common.POCOs;

/// <summary>
/// Defines a POCO representing two tokens, one string, and the other a byte[]
/// </summary>
public class DualToken : POCO
{
    /// <summary>
    /// Typically an authorization (JWT) token
    /// </summary>
    public string TokenA { get; set; }

    /// <summary>
    /// Typically a refresh token
    /// </summary>
    public byte[] TokenB { get; set; }

    public DualToken(string tokenA, byte[] tokenB)
    {
        TokenA = tokenA;
        TokenB = tokenB;
    }
}
