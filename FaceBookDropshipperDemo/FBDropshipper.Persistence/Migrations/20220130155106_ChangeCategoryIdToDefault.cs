using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBDropshipper.Persistence.Migrations
{
    public partial class ChangeCategoryIdToDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_productListings_categories_categoryId",
                table: "productListings");

            migrationBuilder.AlterColumn<int>(
                name: "categoryId",
                table: "productListings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fK_productListings_categories_categoryId",
                table: "productListings",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_productListings_categories_categoryId",
                table: "productListings");

            migrationBuilder.AlterColumn<int>(
                name: "categoryId",
                table: "productListings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fK_productListings_categories_categoryId",
                table: "productListings",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
