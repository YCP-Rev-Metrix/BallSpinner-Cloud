namespace Common.POCOs.MobileApp;

public class User : Poco
{
    public User() { }

    public User(int? id, string firstname, string lastname, string username, byte[] hashedPassword, string email, string phoneNumber, string? lastLogin, string? hand)
    {
        Id = id;
        Firstname = firstname;
        Lastname = lastname;
        Username = username;
        HashedPassword = hashedPassword;
        Email = email;
        PhoneNumber = phoneNumber;
        LastLogin = lastLogin;
        Hand = hand;
    }

    public int? Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Username { get; set; }
    public byte[]? HashedPassword { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LastLogin { get; set; }
    public string? Hand { get; set; }
}
