using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionOutput_MaterialOutput_MaterialOutputId",
                table: "ConsumptionOutput");

            migrationBuilder.DropForeignKey(
                name: "FK_PermanentOutput_MaterialInputByVendor_MaterialInputByVendorId",
                table: "PermanentOutput");

            migrationBuilder.DropForeignKey(
                name: "FK_PermanentOutput_MaterialOutput_MaterialOutputId",
                table: "PermanentOutput");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermanentOutput",
                table: "PermanentOutput");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsumptionOutput",
                table: "ConsumptionOutput");

            migrationBuilder.RenameTable(
                name: "PermanentOutput",
                newName: "AuxiliarPermanent");

            migrationBuilder.RenameTable(
                name: "ConsumptionOutput",
                newName: "AuxiliarConsumption");

            migrationBuilder.RenameIndex(
                name: "IX_PermanentOutput_MaterialOutputId",
                table: "AuxiliarPermanent",
                newName: "IX_AuxiliarPermanent_MaterialOutputId");

            migrationBuilder.RenameIndex(
                name: "IX_PermanentOutput_MaterialInputByVendorId",
                table: "AuxiliarPermanent",
                newName: "IX_AuxiliarPermanent_MaterialInputByVendorId");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumptionOutput_MaterialOutputId",
                table: "AuxiliarConsumption",
                newName: "IX_AuxiliarConsumption_MaterialOutputId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuxiliarPermanent",
                table: "AuxiliarPermanent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuxiliarConsumption",
                table: "AuxiliarConsumption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarConsumption_MaterialOutput_MaterialOutputId",
                table: "AuxiliarConsumption",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarPermanent_MaterialInputByVendor_MaterialInputByVendorId",
                table: "AuxiliarPermanent",
                column: "MaterialInputByVendorId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarPermanent_MaterialOutput_MaterialOutputId",
                table: "AuxiliarPermanent",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarConsumption_MaterialOutput_MaterialOutputId",
                table: "AuxiliarConsumption");

            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarPermanent_MaterialInputByVendor_MaterialInputByVendorId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarPermanent_MaterialOutput_MaterialOutputId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuxiliarPermanent",
                table: "AuxiliarPermanent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuxiliarConsumption",
                table: "AuxiliarConsumption");

            migrationBuilder.RenameTable(
                name: "AuxiliarPermanent",
                newName: "PermanentOutput");

            migrationBuilder.RenameTable(
                name: "AuxiliarConsumption",
                newName: "ConsumptionOutput");

            migrationBuilder.RenameIndex(
                name: "IX_AuxiliarPermanent_MaterialOutputId",
                table: "PermanentOutput",
                newName: "IX_PermanentOutput_MaterialOutputId");

            migrationBuilder.RenameIndex(
                name: "IX_AuxiliarPermanent_MaterialInputByVendorId",
                table: "PermanentOutput",
                newName: "IX_PermanentOutput_MaterialInputByVendorId");

            migrationBuilder.RenameIndex(
                name: "IX_AuxiliarConsumption_MaterialOutputId",
                table: "ConsumptionOutput",
                newName: "IX_ConsumptionOutput_MaterialOutputId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermanentOutput",
                table: "PermanentOutput",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsumptionOutput",
                table: "ConsumptionOutput",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumptionOutput_MaterialOutput_MaterialOutputId",
                table: "ConsumptionOutput",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermanentOutput_MaterialInputByVendor_MaterialInputByVendorId",
                table: "PermanentOutput",
                column: "MaterialInputByVendorId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermanentOutput_MaterialOutput_MaterialOutputId",
                table: "PermanentOutput",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
