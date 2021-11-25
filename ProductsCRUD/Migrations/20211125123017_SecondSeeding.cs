using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsCRUD.Migrations
{
    public partial class SecondSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "_products",
                schema: "staging",
                newName: "_products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "staging");

            migrationBuilder.RenameTable(
                name: "_products",
                newName: "_products",
                newSchema: "staging");
        }
    }
}
