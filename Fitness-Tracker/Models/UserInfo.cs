using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fitness_Tracker.Models
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string User { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; }
        [DisplayName("Table Ready")]
        public bool TableReady { get; set; }


    }
}
