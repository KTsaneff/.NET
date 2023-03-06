using System.ComponentModel.DataAnnotations;

namespace Quizz.Infrastructure.Data.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
