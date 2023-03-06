using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizz.Data.Migrations
{
    public partial class IsActiveAddedToQuizz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Quizzes");
        }
    }
}
