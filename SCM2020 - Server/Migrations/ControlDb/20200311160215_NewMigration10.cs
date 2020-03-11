using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarConsumption_MaterialOutput_MaterialOutputId",
                table: "AuxiliarConsumption");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarConsumption_MaterialOutput_MaterialOutputId",
                table: "AuxiliarConsumption",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarConsumption_MaterialOutput_MaterialOutputId",
                table: "AuxiliarConsumption");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarConsumption_MaterialOutput_MaterialOutputId",
                table: "AuxiliarConsumption",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
