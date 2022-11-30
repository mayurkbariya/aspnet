using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FBDropshipper.Persistence.Migrations
{
    public partial class AddListing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    categoryType = table.Column<int>(type: "integer", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "listingTemplates",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    marketPlaceId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    profitPercent = table.Column<float>(type: "real", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    shippingRate = table.Column<float>(type: "real", nullable: false),
                    deliveryMethod = table.Column<int>(type: "integer", nullable: false),
                    header = table.Column<string>(type: "text", nullable: true),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_listingTemplates", x => x.id);
                    table.ForeignKey(
                        name: "fK_listingTemplates_marketPlaces_marketPlaceId",
                        column: x => x.marketPlaceId,
                        principalTable: "marketPlaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productListings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    listingTemplateId = table.Column<int>(type: "integer", nullable: false),
                    marketPlaceId = table.Column<int>(type: "integer", nullable: false),
                    inventoryProductId = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    header = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<float>(type: "real", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    categoryId = table.Column<int>(type: "integer", nullable: false),
                    listingId = table.Column<string>(type: "text", nullable: true),
                    listingUrl = table.Column<string>(type: "text", nullable: true),
                    listedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deliveryMethod = table.Column<int>(type: "integer", nullable: false),
                    listingStatus = table.Column<int>(type: "integer", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_productListings", x => x.id);
                    table.ForeignKey(
                        name: "fK_productListings_categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_productListings_inventoryProducts_inventoryProductId",
                        column: x => x.inventoryProductId,
                        principalTable: "inventoryProducts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_productListings_listingTemplates_listingTemplateId",
                        column: x => x.listingTemplateId,
                        principalTable: "listingTemplates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_productListings_marketPlaces_marketPlaceId",
                        column: x => x.marketPlaceId,
                        principalTable: "marketPlaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productListingImages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productListingId = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<int>(type: "integer", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_productListingImages", x => x.id);
                    table.ForeignKey(
                        name: "fK_productListingImages_productListings_productListingId",
                        column: x => x.productListingId,
                        principalTable: "productListings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_listingTemplates_marketPlaceId",
                table: "listingTemplates",
                column: "marketPlaceId");

            migrationBuilder.CreateIndex(
                name: "iX_productListingImages_productListingId",
                table: "productListingImages",
                column: "productListingId");

            migrationBuilder.CreateIndex(
                name: "iX_productListings_categoryId",
                table: "productListings",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "iX_productListings_inventoryProductId",
                table: "productListings",
                column: "inventoryProductId");

            migrationBuilder.CreateIndex(
                name: "iX_productListings_listingTemplateId",
                table: "productListings",
                column: "listingTemplateId");

            migrationBuilder.CreateIndex(
                name: "iX_productListings_marketPlaceId",
                table: "productListings",
                column: "marketPlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productListingImages");

            migrationBuilder.DropTable(
                name: "productListings");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "listingTemplates");
        }
    }
}
