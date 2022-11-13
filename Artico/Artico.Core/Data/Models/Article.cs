using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artico.Core.Data.Models
{
    public class Article
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Body { get; set; } = null!;

        [Required]
        public List<string> Tags { get; set; } = new List<string>();

        public int Order { get; set; }

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        public int JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        public Job TargetJob { get; set; } = null!;

        [Required]
        public int PositionId { get; set; }

        [ForeignKey(nameof(PositionId))]
        public Position TargetPosition { get; set; } = null!;

        public bool IsAthorisationNeeded { get; set; } = false;
    }
}
