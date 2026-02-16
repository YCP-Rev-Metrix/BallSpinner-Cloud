using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables;

public class EventTable
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    
    public string Name { get; set; }
    
    public string Type { get; set; }
    
    public string Location { get; set; }
    
    public int Average { get; set; }
    
    public int Stats { get; set; }
    
    public string Standings { get; set; }
}