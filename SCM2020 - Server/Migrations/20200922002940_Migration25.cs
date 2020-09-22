using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class Migration25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stock",
                table: "ConsumptionProduct");

            migrationBuilder.AddColumn<double>(
                name: "stock1",
                table: "ConsumptionProduct",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stock1",
                table: "ConsumptionProduct");

            migrationBuilder.AddColumn<double>(
                name: "stock",
                table: "ConsumptionProduct",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
