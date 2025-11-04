using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables.Team_PI_Tables;

public class HeatData
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("SessionId")]
    public int SessionId { get; set; }
    
    [Required]
    public float Time { get; set; }
    
    [Required]
    public float Value { get; set; }
    
    [Required]
    public float MotorId { get; set; }
}