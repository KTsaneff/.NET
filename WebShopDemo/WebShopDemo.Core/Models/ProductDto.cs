using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [StringLength(50)]
        public Guid Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        [Required]
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
