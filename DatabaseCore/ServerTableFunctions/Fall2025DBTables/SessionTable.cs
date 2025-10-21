using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables
{
    public class SessionTable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int SessionNumber { get; set; }
        [Required]
        [ForeignKey("EstablishmentID")]
        public int EstablishmentID { get; set; }
        [Required]
        [ForeignKey("EventID")]
        public int EventID { get; set; }
        [Required]
        public int DateTime { get; set; }
        [Required]
        public string TeamOpponent { get; set; }
        [Required]
        public string IndividualOpponent { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public int Stats { get; set; }
        [Required]
        public int TeamRecord { get; set; }
        [Required]
        public int IndividualRecord { get; set; }
        

    }
}
