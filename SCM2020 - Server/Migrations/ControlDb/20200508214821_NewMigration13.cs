using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeRegistration",
                table: "MaterialOutput");

            migrationBuilder.DropColumn(
                name: "RequestingSector",
                table: "MaterialOutput");

            migrationBuilder.DropColumn(
                name: "SCMRegistration",
                table: "MaterialOutput");

            migrationBuilder.AddColumn<int>(
                name: "RequestingSector",
                table: "Monitoring",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "MaterialOutput",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCMEmployeeId",
                table: "MaterialOutput",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCMRegistration",
                table: "AuxiliarPermanent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCMRegistration",
                table: "AuxiliarConsumption",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestingSector",
                table: "Monitoring");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "MaterialOutput");

            migrationBuilder.DropColumn(
                name: "SCMEmployeeId",
                table: "MaterialOutput");

            migrationBuilder.DropColumn(
                name: "SCMRegistration",
                table: "AuxiliarPermanent");

            migrationBuilder.DropColumn(
                name: "SCMRegistration",
                table: "AuxiliarConsumption");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeRegistration",
                table: "MaterialOutput",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestingSector",
                table: "MaterialOutput",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SCMRegistration",
                table: "MaterialOutput",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
