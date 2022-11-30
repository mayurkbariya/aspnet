using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FBDropshipper.Persistence.Migrations
{
    public partial class AddOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    marketPlaceId = table.Column<int>(type: "integer", nullable: false),
                    productListingId = table.Column<int>(type: "integer", nullable: false),
                    orderId = table.Column<string>(type: "text", nullable: true),
                    orderUrl = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    subTotal = table.Column<double>(type: "double precision", nullable: false),
                    shipping = table.Column<double>(type: "double precision", nullable: false),
                    fee = table.Column<double>(type: "double precision", nullable: false),
                    supplierOrderId = table.Column<string>(type: "text", nullable: true),
                    supplierCost = table.Column<double>(type: "double precision", nullable: false),
                    trackingCarrier = table.Column<int>(type: "integer", nullable: false),
                    trackingNumber = table.Column<string>(type: "text", nullable: true),
                    orderStatus = table.Column<int>(type: "integer", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_orders", x => x.id);
                    table.ForeignKey(
                        name: "fK_orders_marketPlaces_marketPlaceId",
                        column: x => x.marketPlaceId,
                        principalTable: "marketPlaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_orders_productListings_productListingId",
                        column: x => x.productListingId,
                        principalTable: "productListings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_orders_marketPlaceId",
                table: "orders",
                column: "marketPlaceId");

            migrationBuilder.CreateIndex(
                name: "iX_orders_productListingId",
                table: "orders",
                column: "productListingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}
