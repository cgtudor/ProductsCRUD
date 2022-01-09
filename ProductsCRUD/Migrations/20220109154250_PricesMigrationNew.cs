using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsCRUD.Migrations
{
    public partial class PricesMigrationNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_productPrices",
                columns: table => new
                {
                    ProductPriceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ProductPrice = table.Column<double>(type: "float", nullable: false),
                    PriceChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__productPrices", x => x.ProductPriceID);
                });

            migrationBuilder.CreateTable(
                name: "_products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductPrice = table.Column<double>(type: "float", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__products", x => x.ProductID);
                });

            migrationBuilder.InsertData(
                table: "_productPrices",
                columns: new[] { "ProductPriceID", "PriceChangeDate", "ProductID", "ProductPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 10.0 },
                    { 2, new DateTime(2020, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8.2899999999999991 },
                    { 3, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 11.82 },
                    { 4, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 10.0 },
                    { 5, new DateTime(2020, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 5.7800000000000002 }
                });

            migrationBuilder.InsertData(
                table: "_products",
                columns: new[] { "ProductID", "ProductDescription", "ProductName", "ProductPrice", "ProductQuantity" },
                values: new object[,]
                {
                    { 1, "For his or her sensory pleasure. Fits few known smartphones.", "Rippled Screen Protector", 8.2899999999999991, 4 },
                    { 2, "Poor quality fake faux leather cover, loose enough to fit any mobile device.", "Wrap it and Hope Cover", 5.7800000000000002, 45 },
                    { 3, "Purchase your favourite chocolate and use the provided heating element t melt it into the perfect cover for your phone.", "Chocolate Cover", 11.82, 1243 },
                    { 4, "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", "Water Bath Case", 16.829999999999998, 23 },
                    { 5, "Keep your smartphone handsfree with this large assembly that attaches to your rear window wiper.", "Smartphone Car Holder", 97.019999999999996, 43 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_productPrices");

            migrationBuilder.DropTable(
                name: "_products");
        }
    }
}
