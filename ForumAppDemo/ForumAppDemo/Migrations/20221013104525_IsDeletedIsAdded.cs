using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumAppDemo.Migrations
{
    public partial class IsDeletedIsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Marks record as deleted");

            migrationBuilder.CreateTable(
                name: "PostViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostViewModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostViewModel");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Posts");
        }
    }
}
