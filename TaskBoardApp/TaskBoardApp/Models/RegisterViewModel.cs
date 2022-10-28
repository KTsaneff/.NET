using System.ComponentModel.DataAnnotations;
using TaskBoardApp.Data;

namespace TaskBoardApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(DataConstants.UserUsernameMaxLength, MinimumLength = DataConstants.UserUsernameMinLength)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [StringLength(DataConstants.UserFirstNameMaxLength, MinimumLength = DataConstants.UserFirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(DataConstants.UserLastNameMaxLength, MinimumLength = DataConstants.UserLastNameMinLength)]
        public string LastName { get; set; } = null!;
    }
}
