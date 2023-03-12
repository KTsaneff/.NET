using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime;
using System.Text;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Title { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan Duration { get; set; }

        [Required]
        [Range(typeof(float), "0.00", "10.00")]
        public float Rating { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        [StringLength(700)]
        public string Description { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Screenwriter { get; set; }

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();

        public List<Cast> Casts { get; set; } = new List<Cast>();
    }
}
