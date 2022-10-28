using System.ComponentModel.DataAnnotations;

namespace TaskBoardApp.Data.Entities
{
    public class Board
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxBoardName)]
        public string Name { get; set; } = null!;

        public IEnumerable<BoardTask> BoardTasks { get; set; } = new List<BoardTask>();
    }
}
