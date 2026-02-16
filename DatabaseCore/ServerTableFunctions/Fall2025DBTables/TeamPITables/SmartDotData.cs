using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables.Team_PI_Tables;

public class SmartDotData
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("SessionId")]
    public int SessionId { get; set; }
    
    [Required]
    public float Time { get; set; }
    
    [Required]
    public float AccelX { get; set; }
    
    [Required]
    public float AccelY { get; set; }
    
    [Required]
    public float AccelZ { get; set; }
    
    [Required]
    public float GyroX { get; set; }
    
    [Required]
    public float GyroY { get; set; }
    
    [Required]
    public float GyroZ { get; set; }
    
    [Required]
    public float MagnoX { get; set; }
    
    [Required]
    public float MagnoY { get; set; }
    
    [Required]
    public float MagnoZ { get; set; }
    
    [Required]
    public float Light { get; set; }
    
    [Required]
    public int ReplayIteration { get; set; }
    
    [Required]
    public int DataSelector { get; set; }
}