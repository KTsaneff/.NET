using System.ComponentModel.DataAnnotations;

namespace WebShopDemo.Models
{
    public class LoginViewModel
    {

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [UIHint("hidden")]
        public string? ReturnUrl { get; set; }
    }
}
