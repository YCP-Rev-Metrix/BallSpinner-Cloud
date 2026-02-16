using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables
{
    public class GameTable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string GameNumber { get; set; }
        [Required]
        public string Lanes { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public int Win { get; set; }
        [Required]
        public int StartingLane { get; set; }
        [Required]
        [ForeignKey("SessionID")]
        public int SessionID { get; set; }
        [Required]
        public int TeamResult { get; set; }
        [Required]
        public int IndividualResult { get; set; }

    }
}
