using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables;

public class BallTable
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string Name { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string Weight { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string CoreType { get; set; }
}