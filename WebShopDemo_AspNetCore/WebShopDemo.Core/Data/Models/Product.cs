using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebShopDemo.Core.Data.Models
{
    [Comment("PRoducts to sell")]
    public class Product
    {
        [Key]
        [Comment("Primary key")]
        public Guid Id { get; set; }

        [Required]
        [Comment("Product name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Comment("Product price")]
        public decimal Price { get; set; }

        [Required]
        [Comment("Products in stock")]
        public int Quantity { get; set; }

        [Comment("Flag to assure if a product has active status")]
        public bool IsActive { get; set; }
    }
}
