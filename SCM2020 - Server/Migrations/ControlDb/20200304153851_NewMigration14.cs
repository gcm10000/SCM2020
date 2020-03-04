using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "ConsumptionProduct");

            migrationBuilder.DropColumn(
                name: "Drawer",
                table: "ConsumptionProduct");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "ConsumptionProduct");

            migrationBuilder.AlterColumn<string>(
                name: "Localization",
                table: "ConsumptionProduct",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "NumberLocalization",
                table: "ConsumptionProduct",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberLocalization",
                table: "ConsumptionProduct");

            migrationBuilder.AlterColumn<int>(
                name: "Localization",
                table: "ConsumptionProduct",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "ConsumptionProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Drawer",
                table: "ConsumptionProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Vendor",
                table: "ConsumptionProduct",
                type: "int",
                nullable: true);
        }
    }
}
