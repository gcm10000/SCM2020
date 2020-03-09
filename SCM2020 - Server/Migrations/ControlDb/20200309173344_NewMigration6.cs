using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanent_ProductId",
                table: "AuxiliarPermanent",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarConsumption_ProductId",
                table: "AuxiliarConsumption",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarConsumption_MaterialInputByVendor_ProductId",
                table: "AuxiliarConsumption",
                column: "ProductId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarPermanent_MaterialInputByVendor_ProductId",
                table: "AuxiliarPermanent",
                column: "ProductId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarConsumption_MaterialInputByVendor_ProductId",
                table: "AuxiliarConsumption");

            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarPermanent_MaterialInputByVendor_ProductId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropIndex(
                name: "IX_AuxiliarPermanent_ProductId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropIndex(
                name: "IX_AuxiliarConsumption_ProductId",
                table: "AuxiliarConsumption");

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "AuxiliarConsumption",
                type: "int",
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
    }
}
