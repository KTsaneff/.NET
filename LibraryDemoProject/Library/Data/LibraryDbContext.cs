using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library.Data.Models;
using Library.Common;

namespace Library.Data
{
    public class LibraryDbContext : IdentityDbContext<User>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserBook>()
                .HasKey(x => new { x.UserId, x.BookId });

            builder.Entity<User>()
                .Property(u => u.UserName)
                .HasMaxLength(ValidationConstants.UserUsernameMaxLength)
                .IsRequired();

            builder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(ValidationConstants.UserEmailMaxLength)
                .IsRequired();

            builder
                .Entity<Category>()
                .HasData(new Category()
                {
                    Id = 1,
                    Name = "Action"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Biography"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Children"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Crime"
                },
                new Category()
                {
                    Id = 5,
                    Name = "Fantasy"
                });


            base.OnModelCreating(builder);
        }
    }
}