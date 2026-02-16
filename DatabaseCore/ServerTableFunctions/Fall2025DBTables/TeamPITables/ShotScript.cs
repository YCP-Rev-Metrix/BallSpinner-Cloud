using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables.Team_PI_Tables;

public class ShotScript
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("SessionId")]
    public int SessionId { get; set; }
    
    [Required]
    public float Time { get; set; }
    
    [Required]
    public float Rpm { get; set; }
    
    [Required]
    public float AngleDegrees { get; set; }
    
    [Required]
    public float TiltDegrees { get; set; }
}