using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ApplicationDb
{
    public partial class NewMigrate15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdSector",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdSector",
                table: "AspNetUsers");
        }
    }
}
