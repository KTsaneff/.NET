using Library.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.BookTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.BookAuthorMaxLength)]
        public string Author { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.BookDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public decimal Rating { get; set; }

        [Required]
        public int CategotyId { get; set; }

        [ForeignKey(nameof(CategotyId))]
        public Category Category { get; set; } = null!;

        public List<UserBook> UsersBooks { get; set; } = new List<UserBook>();
    }
}
