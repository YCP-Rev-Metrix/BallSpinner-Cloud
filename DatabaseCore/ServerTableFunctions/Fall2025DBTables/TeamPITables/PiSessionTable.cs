using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; 

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables.Team_PI_Tables;

public class PiSessionTable
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTime TimeStamp { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public bool IsShotMode { get; set; }
}