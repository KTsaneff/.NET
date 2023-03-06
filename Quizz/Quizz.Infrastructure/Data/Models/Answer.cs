using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizz.Infrastructure.Data.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        public int QuestionId { get; set; }

        [Required]
        [StringLength(500)]
        public string AnswerText { get; set; } = null!;

        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; } = null!;
    }
}
