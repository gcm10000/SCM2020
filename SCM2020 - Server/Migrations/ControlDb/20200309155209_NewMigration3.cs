using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionProduct_MaterialInputByVendor_MaterialInputByVendorId",
                table: "ConsumptionProduct");

            migrationBuilder.DropIndex(
                name: "IX_ConsumptionProduct_MaterialInputByVendorId",
                table: "ConsumptionProduct");

            migrationBuilder.DropColumn(
                name: "MaterialInputByVendorId",
                table: "ConsumptionProduct");

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "AuxiliarConsumption",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarConsumption_MaterialInputByVendorId",
                table: "AuxiliarConsumption",
                column: "MaterialInputByVendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarConsumption_MaterialInputByVendor_MaterialInputByVendorId",
                table: "AuxiliarConsumption",
                column: "MaterialInputByVendorId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarConsumption_MaterialInputByVendor_MaterialInputByVendorId",
                table: "AuxiliarConsumption");

            migrationBuilder.DropIndex(
                name: "IX_AuxiliarConsumption_MaterialInputByVendorId",
                table: "AuxiliarConsumption");

            migrationBuilder.DropColumn(
                name: "MaterialInputByVendorId",
                table: "AuxiliarConsumption");

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "ConsumptionProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionProduct_MaterialInputByVendorId",
                table: "ConsumptionProduct",
                column: "MaterialInputByVendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumptionProduct_MaterialInputByVendor_MaterialInputByVendorId",
                table: "ConsumptionProduct",
                column: "MaterialInputByVendorId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
