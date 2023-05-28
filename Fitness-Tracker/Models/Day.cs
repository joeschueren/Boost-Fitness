using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fitness_Tracker.Models
{
    public class Day
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public int CaloriesIn { get; set; }
        [Required]
        public int MinExercise { get; set; }
        [Required]
        public string Date { get; set; }
        [DisplayName("Table Ready")]
        public bool TableReady { get; set; }
    }
}
