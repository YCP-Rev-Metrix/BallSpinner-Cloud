using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables;

public class BallTable
{
    [Key]
    public int Id { get; set; }

    public int? MobileID { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [MaxLength(50)]
    [Required]
    public string Name { get; set; }

    [MaxLength(100)]
    public string BallMFG { get; set; }

    [MaxLength(100)]
    public string BallMFGName { get; set; }

    [MaxLength(100)]
    public string SerialNumber { get; set; }

    public int? Weight { get; set; }

    [MaxLength(100)]
    public string Core { get; set; }

    [MaxLength(50)]
    [Required]
    public string ColorString { get; set; }

    [MaxLength(100)]
    public string Coverstock { get; set; }

    [MaxLength(500)]
    public string Comment { get; set; }

    [Required]
    public bool Enabled { get; set; }
}
