using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsCRUD.Migrations
{
    public partial class InitialSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "staging");

            migrationBuilder.RenameTable(
                name: "_products",
                newName: "_products",
                newSchema: "staging");

            migrationBuilder.InsertData(
                schema: "staging",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "staging",
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -5);

            migrationBuilder.DeleteData(
                schema: "staging",
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                schema: "staging",
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                schema: "staging",
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                schema: "staging",
                table: "_products",
                keyColumn: "ProductID",
                keyValue: -1);

            migrationBuilder.RenameTable(
                name: "_products",
                schema: "staging",
                newName: "_products");
        }
    }
}
