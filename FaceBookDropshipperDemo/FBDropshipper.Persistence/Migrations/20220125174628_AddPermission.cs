using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBDropshipper.Persistence.Migrations
{
    public partial class AddPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "inventoryProducts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "catalogProducts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "teamMemberPermission",
                columns: table => new
                {
                    userId = table.Column<string>(type: "text", nullable: false),
                    marketPlaceId = table.Column<int>(type: "integer", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_teamMemberPermission", x => new { x.userId, x.marketPlaceId });
                    table.ForeignKey(
                        name: "fK_teamMemberPermission_marketPlaces_marketPlaceId",
                        column: x => x.marketPlaceId,
                        principalTable: "marketPlaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_teamMemberPermission_users_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_teamMemberPermission_marketPlaceId",
                table: "teamMemberPermission",
                column: "marketPlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "teamMemberPermission");

            migrationBuilder.DropColumn(
                name: "price",
                table: "inventoryProducts");

            migrationBuilder.DropColumn(
                name: "price",
                table: "catalogProducts");
        }
    }
}
