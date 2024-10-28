namespace Common.POCOs;

/// <summary>
/// Defines a POCO representing a user's identification
/// </summary>
public class UserIdentification : POCO
{
    public UserIdentification() { }

    public UserIdentification(string firstname, string lastname, string username, string password, string email, string phone_number)
    {
        Firstname = firstname;
        Lastname = lastname;
        Username = username;
        Password = password;
        Email = email;
        Phone_number = phone_number;
    }

    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone_number { get; set; }

}
