namespace Common.POCOs;

/// <summary>
/// Defines a POCO representing only a string for a plaintext password
/// </summary>
public class Password : POCO
{
    public string RawPassword { set; get; }

    public Password(string rawPassword) => RawPassword = rawPassword;
}
