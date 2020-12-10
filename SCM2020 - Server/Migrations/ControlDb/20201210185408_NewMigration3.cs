using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberLocalization",
                table: "ConsumptionProduct");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NumberLocalization",
                table: "ConsumptionProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
