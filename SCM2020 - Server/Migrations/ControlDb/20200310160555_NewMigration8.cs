using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SCMEmployeeId",
                table: "MaterialInputByVendor",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Invoice",
                table: "MaterialInputByVendor",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "AuxiliarConsumption",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AuxiliarConsumption");

            migrationBuilder.AlterColumn<string>(
                name: "SCMEmployeeId",
                table: "MaterialInputByVendor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Invoice",
                table: "MaterialInputByVendor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
