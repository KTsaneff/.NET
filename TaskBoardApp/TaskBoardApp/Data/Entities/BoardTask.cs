using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBoardApp.Data.Entities
{
    public class BoardTask
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxTaskTitle)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DataConstants.MaxTaskDescription)]
        public string Description { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        [Required]
        public Guid BoardId { get; set; }

        [ForeignKey(nameof(BoardId))]
        public Board Board { get; set; } = null!;

        [Required]
        public string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public TaskBoardUser Owner { get; set; } = null!;
    }
}
