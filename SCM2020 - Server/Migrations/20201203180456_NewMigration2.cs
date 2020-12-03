using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class NewMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "GroupEmployees");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GroupEmployees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupEmployees");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "GroupEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
