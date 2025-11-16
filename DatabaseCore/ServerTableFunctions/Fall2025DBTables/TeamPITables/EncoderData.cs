using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables.Team_PI_Tables;

public class EncoderData
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("SessionId")]
    public int SessionId { get; set; }
    
    [Required]
    public float Time { get; set; }
    
    [Required]
    public float Pulses { get; set; }
    
    [Required]
    public int MotorId { get; set; }
}