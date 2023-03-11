using System.ComponentModel.DataAnnotations;

namespace Quizz.Core.Models
{
    public class QuizzViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 10)]
        public string Name { get; set; } = null!;
    }
}
