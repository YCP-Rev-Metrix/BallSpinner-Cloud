using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables;

public class FrameTable
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("GameId")]
    public int GameId { get; set; }
    
    [ForeignKey("ShotOne")]
    public int ShotOne { get; set; }
    
    [ForeignKey("ShotTwo")]
    public int ShotTwo { get; set; }
    
    [Required]
    public int FrameNumber { get; set; }
    
    [Required]
    public int Lane { get; set; }
    
    [Required]
    public int Result { get; set; }
}