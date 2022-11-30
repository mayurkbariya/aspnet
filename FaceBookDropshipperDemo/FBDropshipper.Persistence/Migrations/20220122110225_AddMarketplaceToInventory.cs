using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBDropshipper.Persistence.Migrations
{
    public partial class AddMarketplaceToInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "marketPlaceId",
                table: "inventoryProducts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "iX_inventoryProducts_marketPlaceId",
                table: "inventoryProducts",
                column: "marketPlaceId");

            migrationBuilder.AddForeignKey(
                name: "fK_inventoryProducts_marketPlaces_marketPlaceId",
                table: "inventoryProducts",
                column: "marketPlaceId",
                principalTable: "marketPlaces",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_inventoryProducts_marketPlaces_marketPlaceId",
                table: "inventoryProducts");

            migrationBuilder.DropIndex(
                name: "iX_inventoryProducts_marketPlaceId",
                table: "inventoryProducts");

            migrationBuilder.DropColumn(
                name: "marketPlaceId",
                table: "inventoryProducts");
        }
    }
}
