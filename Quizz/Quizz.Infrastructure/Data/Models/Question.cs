using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizz.Infrastructure.Data.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        public int QuizId { get; set; }

        [Required]
        [StringLength(500)]
        public string QuestionText { get; set; } = null!;

        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; } = null!;

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
