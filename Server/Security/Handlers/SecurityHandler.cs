using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Server.Security.Handlers;

/// <summary>
/// Provides basic security items for the server
/// </summary>
public class SecurityHandler
{
    /// <summary>
    /// Secure random number generator
    /// </summary>
    private readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    /// <summary>
    /// Key used to sign JWTs
    /// </summary>
    public readonly SymmetricSecurityKey AuthorizationSigningTokenKey;

    /// <summary>
    /// Credentials used to sign JWTs
    /// </summary>
    public readonly SigningCredentials AuthorizationSigningCredentials;

    public SecurityHandler()
    {
        AuthorizationSigningTokenKey = GenerateSymmetricKey(Config.AuthSecretLength);
        AuthorizationSigningCredentials = new(AuthorizationSigningTokenKey, SecurityAlgorithms.HmacSha256Signature);
    }

    /// <summary>
    /// Securely generates N random bytes
    /// </summary>
    /// <param name="length">Length of Byte[] to generate</param>
    /// <returns>Secure random bytes</returns>
    public byte[] GenerateRandomBytes(int length)
    {
        byte[] randomBytes = new byte[length];
        RandomNumberGenerator.GetBytes(randomBytes);
        return randomBytes;
    }

    /// <summary>
    /// Generates a new refresh token, 32 bytes long
    /// </summary>
    /// <returns>Refresh token</returns>
    public byte[] GenerateRefreshToken() => GenerateRandomBytes(32);

    private SymmetricSecurityKey GenerateSymmetricKey(int length) => new(GenerateRandomBytes(length));

    /// <summary>
    /// Salts and hashes the provided plaintext password
    /// </summary>
    /// <param name="password">Plaintext password to salt and hash</param>
    /// <returns>The hashed password as well as the used salt</returns>
    public (byte[] hashed, byte[] salt) SaltHashPassword(string password)
    {
        byte[] salt = GenerateRandomBytes(16);
        byte[] hashed = SaltHashPassword(password, salt);
        return (hashed, salt);
    }

    /// <summary>
    /// Hashes the provided password using the provided salt
    /// </summary>
    /// <param name="password">Plaintext password to hash</param>
    /// <param name="salt">Salt used previously on this password</param>
    /// <returns>The hashed password</returns>
    public byte[] SaltHashPassword(string password, byte[] salt)
    {
        byte[] passwordbytes = Encoding.ASCII.GetBytes(password);
        var s = new MemoryStream();
        s.Write(passwordbytes, 0, passwordbytes.Length);
        s.Write(salt, 0, salt.Length);
        byte[] combined = s.ToArray();
        byte[] hashed = SHA256.HashData(combined);
        return hashed;
    }
}
