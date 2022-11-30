using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBDropshipper.Persistence.Migrations
{
    public partial class AddShippingRateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "shippingRate",
                table: "productListings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shippingRate",
                table: "productListings");
        }
    }
}
