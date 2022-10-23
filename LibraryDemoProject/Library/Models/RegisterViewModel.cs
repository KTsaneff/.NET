using Library.Common;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(ValidationConstants.UserUsernameMaxLength, MinimumLength = ValidationConstants.UserUsernameMinLength)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(ValidationConstants.UserEmailMaxLength, MinimumLength = ValidationConstants.UserEmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(ValidationConstants.UserPasswordMaxLength, MinimumLength = ValidationConstants.UserPasswordMinLength)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
