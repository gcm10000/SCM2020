using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class NewMigration18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "PermanentProduct");

            migrationBuilder.AddColumn<string>(
                name: "WorkOrder",
                table: "PermanentProduct",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkOrder",
                table: "PermanentProduct");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "PermanentProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
