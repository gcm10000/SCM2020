using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarConsumption_MaterialInput_MaterialInputId",
                table: "AuxiliarConsumption");

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputId",
                table: "AuxiliarPermanent",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanent_MaterialInputId",
                table: "AuxiliarPermanent",
                column: "MaterialInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarConsumption_MaterialInput_MaterialInputId",
                table: "AuxiliarConsumption",
                column: "MaterialInputId",
                principalTable: "MaterialInput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarPermanent_MaterialInput_MaterialInputId",
                table: "AuxiliarPermanent",
                column: "MaterialInputId",
                principalTable: "MaterialInput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarConsumption_MaterialInput_MaterialInputId",
                table: "AuxiliarConsumption");

            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarPermanent_MaterialInput_MaterialInputId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropIndex(
                name: "IX_AuxiliarPermanent_MaterialInputId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropColumn(
                name: "MaterialInputId",
                table: "AuxiliarPermanent");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarConsumption_MaterialInput_MaterialInputId",
                table: "AuxiliarConsumption",
                column: "MaterialInputId",
                principalTable: "MaterialInput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
