using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables;

public class UserTable
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string Firstname { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string Lastname { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string Username { get; set; }
    
    [Required]
    public byte[] HashedPassword { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [MaxLength(12)]
    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string LastLogin { get; set; }

    [Required]
    public string Hand { get; set; }
}