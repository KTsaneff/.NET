using System.ComponentModel.DataAnnotations;

namespace WebShopDemo.Core.Models
{
    /// <summary>
    /// Product model
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
        /// Product price
        /// </summary>
        [Required]
        [Range(typeof(decimal), "0.01", "100000", ConvertValueInInvariantCulture = true)]
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity in stock
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
