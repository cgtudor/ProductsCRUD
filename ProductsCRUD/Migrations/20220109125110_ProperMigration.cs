using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsCRUD.Migrations
{
    public partial class ProperMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -1);

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
            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "_products",
                keyColumn: "ProductID",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "_products",
                columns: new[] { "ProductID", "ProductDescription", "ProductName", "ProductPrice", "ProductQuantity" },
                values: new object[,]
                {
                    { -1, "Tasty and savory.", "Chocolate", 1.5600000000000001, 4 },
                    { -2, "Blonde beer. Yummy.", "Heineken 0.5L", 2.5600000000000001, 45 },
                    { -3, "Fresh.", "Bread", 0.56000000000000005, 1243 },
                    { -4, "Pack of 50 nails.", "Nails", 4.5, 23 },
                    { -5, "Aroma like you've never smelled before.", "Candle", 3.5600000000000001, 43 }
                });
        }
    }
}
