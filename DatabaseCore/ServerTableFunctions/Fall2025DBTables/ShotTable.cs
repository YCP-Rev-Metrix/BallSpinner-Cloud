using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables
{
    public class ShotTable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int SmartDotID { get; set; }
        [Required]
        [ForeignKey("SessionID")]
        public int SesssionID { get; set; }
        [Required]
        [ForeignKey("BallID")]
        public int BallID { get; set; }
        [Required]
        [ForeignKey("FrameID")]
        public int FrameID { get; set; }
        [Required]
        public int ShotNumber { get; set; }
        [Required]
        public int LeaveType { get; set; }
        [Required]
        public string Side { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
