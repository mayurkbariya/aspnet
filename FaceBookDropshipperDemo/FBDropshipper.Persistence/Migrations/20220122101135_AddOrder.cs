using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBDropshipper.Persistence.Migrations
{
    public partial class AddOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "inventoryProductImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "catalogProductImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                table: "inventoryProductImages");

            migrationBuilder.DropColumn(
                name: "order",
                table: "catalogProductImages");
        }
    }
}
