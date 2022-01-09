using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsCRUD.Migrations
{
    public partial class PriceHistoryMigration : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_productPrices");
        }
    }
}
