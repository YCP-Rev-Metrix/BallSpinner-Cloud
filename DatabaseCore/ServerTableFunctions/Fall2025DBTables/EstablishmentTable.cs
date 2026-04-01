using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables;

public class EstablishmentTable
{
    [Key]
    public int ID { get; set; }

    public int? MobileID { get; set; }

    [ForeignKey("UserId")]
    public int? UserId { get; set; }

    [MaxLength(100)]
    [Required]
    public string FullName { get; set; }

    [MaxLength(100)]
    public string NickName { get; set; }

    [MaxLength(200)]
    public string GPSLocation { get; set; }

    public bool HomeHouse { get; set; }

    [MaxLength(200)]
    public string Reason { get; set; }

    [MaxLength(200)]
    public string Address { get; set; }

    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    [MaxLength(50)]
    public string Lanes { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }

    [MaxLength(100)]
    public string Location { get; set; }

    [Required]
    public bool Enabled { get; set; }
}
