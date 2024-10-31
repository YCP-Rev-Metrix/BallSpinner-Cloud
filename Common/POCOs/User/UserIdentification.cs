using System.ComponentModel.DataAnnotations;
using Common.POCOs;

namespace RevMetrix.BallSpinner.BackEnd.Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class UserIdentification: Poco
{
        public UserIdentification() { }

        public UserIdentification(string? firstname, string? lastname, string? username, string? password, string? email, string? phoneNumber)
        {
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        
        [MaxLength(50)]
        public string? Firstname { get; set; }

        [MaxLength(50)]
        public string? Lastname { get; set; }

        [Required, MaxLength(50)]
        public string? Username { get; set; }
        [Required, MaxLength(50)]
        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(10)]
        public string? PhoneNumber { get; set; }

}