using Artico.Core.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Artico.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder
                .Entity<Job>()
                .HasData(new Job()
                {
                    Id = 1,
                    Name = "Software developer"
                },
                new Job()
                {
                    Id = 2,
                    Name = "Computer network architect"
                },
                new Job()
                {
                    Id = 3,
                    Name = "Computer support specialist"
                },
                new Job()
                {
                    Id = 4,
                    Name = "IT project manager"
                },
                new Job()
                {
                    Id = 5,
                    Name = "Web developer"
                },
                new Job()
                {
                    Id = 6,
                    Name = "Information security analyst"
                },
                new Job()
                {
                    Id = 7,
                    Name = "Computer systems analyst"
                },
                new Job()
                {
                    Id = 8,
                    Name = "Database administrators and architect"
                });

            builder
                .Entity<Position>()
                .HasData(new Position()
                {
                    Id = 1,
                    Name = "Executive Director or CEO"
                },
                new Position()
                {
                    Id = 2,
                    Name = "President and Vice-President"
                },
                new Position()
                {
                    Id = 3,
                    Name = "Department director"
                },
                new Position()
                {
                    Id = 4,
                    Name = "Manager"
                },
                new Position()
                {
                    Id = 5,
                    Name = "Supervisor"
                },
                new Job()
                {
                    Id = 6,
                    Name = "Employee"
                },
                new Job()
                {
                    Id = 7,
                    Name = "Intern"
                });
        }

        public DbSet<Article> Articles { get; set; } = null!;

        public DbSet<Job> Jobs { get; set; } = null!;

        public DbSet<Position> Positions { get; set; } = null!;
    }
}