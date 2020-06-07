using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class NewMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SCMRegistration",
                table: "AuxiliarPermanent");

            migrationBuilder.DropColumn(
                name: "SCMRegistration",
                table: "AuxiliarConsumption");

            migrationBuilder.AddColumn<string>(
                name: "SCMEmployeeId",
                table: "AuxiliarPermanent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCMEmployeeId",
                table: "AuxiliarConsumption",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SCMEmployeeId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropColumn(
                name: "SCMEmployeeId",
                table: "AuxiliarConsumption");

            migrationBuilder.AddColumn<string>(
                name: "SCMRegistration",
                table: "AuxiliarPermanent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCMRegistration",
                table: "AuxiliarConsumption",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
