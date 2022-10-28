using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data.Entities;

namespace TaskBoardApp.Data
{
    public class TaskBoardAppDbContext : IdentityDbContext<TaskBoardUser>
    {
        private TaskBoardUser GuestUser { get; set; }

        private Board OpenBoard { get; set; }

        private Board InProgressBoard { get; set; }

        private Board DoneBoard { get; set; }

        public TaskBoardAppDbContext(DbContextOptions<TaskBoardAppDbContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }

        public DbSet<BoardTask> BoardTasks { get; set; }

        public DbSet<Board> Boards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BoardTask>()
                .HasOne(t => t.Board)
                .WithMany(b => b.BoardTasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedUsers();
            builder
                .Entity<TaskBoardUser>()
                .HasData(this.GuestUser);

            SeedBoards();
            builder
                .Entity<Board>()
                .HasData(this.OpenBoard, this.InProgressBoard, this.DoneBoard);

            builder
                .Entity<BoardTask>()
                .HasData(            
                new BoardTask()
                {
                    Id = Guid.NewGuid(),
                    Title = "Prepare for ASP.NET fundamentals exam",
                    Description = "Learn using ASP.NET Core Identity",
                    CreatedOn = DateTime.Now.AddMonths(-1),
                    OwnerId = this.GuestUser.Id,
                    BoardId = this.OpenBoard.Id
                },
                new BoardTask()
                {
                    Id = Guid.NewGuid(),
                    Title = "Improve ASP.NET Core skills",
                    Description = "Improve your skills using ASP.NET Core Identity",
                    CreatedOn = DateTime.Now.AddMonths(-5),
                    OwnerId = this.GuestUser.Id,
                    BoardId = this.OpenBoard.Id
                },
                new BoardTask()
                {
                    Id = Guid.NewGuid(),
                    Title = "Improve EF Core skills",
                    Description = "Learn using EF Core and MS SQL Server Management Studio",
                    CreatedOn = DateTime.Now.AddMonths(-10),
                    OwnerId = this.GuestUser.Id,
                    BoardId = this.OpenBoard.Id
                },
                new BoardTask()
                {
                    Id = Guid.NewGuid(),
                    Title = "Prepare for C# Fundamentals Exam",
                    Description = "Prepare for solving Mid and Final exams",
                    CreatedOn = DateTime.Now.AddYears(-2),
                    OwnerId = this.GuestUser.Id,
                    BoardId = this.OpenBoard.Id
                });

            base.OnModelCreating(builder);
        }

        private void SeedBoards()
        {
            this.OpenBoard = new Board()
            {
                Id = Guid.NewGuid(),
                Name = "Open"
            };
            this.InProgressBoard = new Board()
            {
                Id = Guid.NewGuid(),
                Name = "In Progress"
            };
            this.DoneBoard = new Board()
            {
                Id = Guid.NewGuid(),
                Name = "Done"
            };
        }

        private void SeedUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();

            this.GuestUser = new TaskBoardUser()
            {
                UserName = "guest",
                NormalizedUserName = "GUEST",
                Email = "guest@gmail.com",
                NormalizedEmail = "GUEST@GMAIL.COM",
                FirstName = "Guest",
                LastName = "User"
            };

            this.GuestUser.PasswordHash = hasher.HashPassword(this.GuestUser, "guest");
        }
    }
}