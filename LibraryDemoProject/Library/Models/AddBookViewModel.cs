using Library.Common;
using Library.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AddBookViewModel
    {
        [Required]
        [StringLength(ValidationConstants.BookTitleMaxLength, MinimumLength = ValidationConstants.BookTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ValidationConstants.BookAuthorMaxLength, MinimumLength = ValidationConstants.BookAuthorMinLength)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(ValidationConstants.BookDescriptionMaxLength, MinimumLength = ValidationConstants.BookDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), ValidationConstants.BookRatingMinValue, ValidationConstants.BookRatingMaxValue)]
        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
