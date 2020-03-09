using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "AuxiliarPermanent",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "AuxiliarConsumption",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanent_MaterialInputByVendorId",
                table: "AuxiliarPermanent",
                column: "MaterialInputByVendorId");

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuxiliarPermanent_MaterialInputByVendor_MaterialInputByVendorId",
                table: "AuxiliarPermanent",
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

            migrationBuilder.DropForeignKey(
                name: "FK_AuxiliarPermanent_MaterialInputByVendor_MaterialInputByVendorId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropIndex(
                name: "IX_AuxiliarPermanent_MaterialInputByVendorId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropIndex(
                name: "IX_AuxiliarConsumption_MaterialInputByVendorId",
                table: "AuxiliarConsumption");

            migrationBuilder.DropColumn(
                name: "MaterialInputByVendorId",
                table: "AuxiliarPermanent");

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
    }
}
