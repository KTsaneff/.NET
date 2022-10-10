using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopDemo.Core.Migrations
{
    public partial class IsActiveAddedToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true,
                comment: "Flag to assure if a product has active status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");
        }
    }
}
