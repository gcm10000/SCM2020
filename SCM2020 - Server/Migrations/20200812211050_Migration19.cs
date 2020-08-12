using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class Migration19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceLocation",
                table: "MaterialOutput");

            migrationBuilder.AddColumn<string>(
                name: "ServiceLocation",
                table: "Monitoring",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceLocation",
                table: "Monitoring");

            migrationBuilder.AddColumn<string>(
                name: "ServiceLocation",
                table: "MaterialOutput",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
