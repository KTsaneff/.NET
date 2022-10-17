using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShopDemo.Core.Data.Models;

namespace WebShopDemo.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().Property(p => p.IsDeleted).HasDefaultValue(false);
            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");

            base.OnModelCreating(builder);
        }

        public DbSet<Product> Products { get; set; } = null!;

    }
}