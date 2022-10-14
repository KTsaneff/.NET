using ForumAppDemo.Data.Configure;
using ForumAppDemo.Data.Models;
using Microsoft.EntityFrameworkCore;
using ForumAppDemo.Models;

namespace ForumAppDemo.Data
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PostConfiguration());
            builder.Entity<Post>()
                .Property(p => p.IsDeleted).HasDefaultValue(false);

            base.OnModelCreating(builder);
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<ForumAppDemo.Models.PostViewModel> PostViewModel { get; set; }
    }
}
