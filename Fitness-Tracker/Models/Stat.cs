using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fitness_Tracker.Models
{
    public class Stat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public int TotalBurned { get; set; }
        [Required]
        public int TotalMinutes { get; set; }
        [Required]
        public int ActivityLevel { get; set; }
        [Required]
        public int Goal { get; set; }
        [Required]
        public bool TableReady { get; set; }


    }
}
