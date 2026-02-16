using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.ServerTableFunctions.Fall2025DBTables
{
    public class EstablishmentTable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Lanes { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Location { get; set; }
    }
}
