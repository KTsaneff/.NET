using System.ComponentModel.DataAnnotations;

namespace WebShopDemo.Core.Models
{
    /// <summary>
    /// Product transfer model
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Product identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Product image
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        [Range(typeof(decimal), "0.10", "100000", ConvertValueInInvariantCulture = true)]
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity in stock
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
