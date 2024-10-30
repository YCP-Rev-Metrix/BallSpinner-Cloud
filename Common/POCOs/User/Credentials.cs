namespace Common.POCOs;

public class Credentials
{
    /// <summary>
    /// Typically an authorization (JWT) token
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Typically a refresh token
    /// </summary>
    public string? Password { get; set; }

    public Credentials(string? username, string? password)
    {
        Username = username;
        Password = password;
    }
}
