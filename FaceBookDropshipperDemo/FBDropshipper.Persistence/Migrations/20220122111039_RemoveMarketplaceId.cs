using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBDropshipper.Persistence.Migrations
{
    public partial class RemoveMarketplaceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_marketPlaces_users_UserId",
                table: "marketPlaces");

            migrationBuilder.DropIndex(
                name: "iX_marketPlaces_userId",
                table: "marketPlaces");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "marketPlaces");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "marketPlaces",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "iX_marketPlaces_userId",
                table: "marketPlaces",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "fK_marketPlaces_users_UserId",
                table: "marketPlaces",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "id");
        }
    }
}
