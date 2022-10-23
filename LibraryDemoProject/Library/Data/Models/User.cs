using Microsoft.AspNetCore.Identity;

namespace Library.Data.Models
{
    public class User : IdentityUser
    {
        public List<UserBook> UsersBooks { get; set; } = new List<UserBook>();
    }
}
