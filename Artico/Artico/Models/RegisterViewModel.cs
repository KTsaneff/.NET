using Artico.Core.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Artico.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public int JobId { get; set; }

        public IEnumerable<Job> Jobs { get; set; } = new List<Job>();

        public int PositionId { get; set; }

        public IEnumerable<Position> Positions { get; set; } = new List<Position>();
    }
}
