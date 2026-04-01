using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables;

public class EventTable
{
    [Key]
    public int Id { get; set; }

    public int? MobileID { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [MaxLength(200)]
    [Required]
    public string LongName { get; set; }

    [MaxLength(100)]
    public string NickName { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }

    [MaxLength(100)]
    public string Location { get; set; }

    [MaxLength(20)]
    public string StartDate { get; set; }

    [MaxLength(20)]
    public string EndDate { get; set; }

    [MaxLength(20)]
    public string WeekDay { get; set; }

    [MaxLength(10)]
    public string StartTime { get; set; }

    public int NumGamesPerSession { get; set; }

    public int? Average { get; set; }

    [MaxLength(500)]
    public string Schedule { get; set; }

    public int? Stats { get; set; }

    [MaxLength(500)]
    public string Standings { get; set; }

    [Required]
    public bool Enabled { get; set; }
}
