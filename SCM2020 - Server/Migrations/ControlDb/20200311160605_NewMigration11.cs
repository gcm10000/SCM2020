using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarPermanent_MaterialOutput_MaterialOutputId",
                table: "AuxiliarPermanent");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarPermanent_MaterialOutput_MaterialOutputId",
                table: "AuxiliarPermanent",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarPermanent_MaterialOutput_MaterialOutputId",
                table: "AuxiliarPermanent");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarPermanent_MaterialOutput_MaterialOutputId",
                table: "AuxiliarPermanent",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
