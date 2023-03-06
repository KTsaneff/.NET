using Artico.Core.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artico.Models
{
    public class CreateArticleViewModel
    {

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        public string Body { get; set; } = null!;
                
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public int Order { get; set; }

        public int JobId { get; set; }

        public IEnumerable<Job> Jobs { get; set; } = new List<Job>();

        public int PositionId { get; set; }

        public IEnumerable<Position> Positions { get; set; } = new List<Position>();

        public bool IsAthorisationNeeded { get; set; } = false;
    }
}
