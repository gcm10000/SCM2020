using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class Migration22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "AuxiliarPermanent",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanent_MaterialInputByVendorId",
                table: "AuxiliarPermanent",
                column: "MaterialInputByVendorId");

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
                name: "FK_AuxiliarPermanent_MaterialInputByVendor_MaterialInputByVendorId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropIndex(
                name: "IX_AuxiliarPermanent_MaterialInputByVendorId",
                table: "AuxiliarPermanent");

            migrationBuilder.DropColumn(
                name: "MaterialInputByVendorId",
                table: "AuxiliarPermanent");
        }
    }
}
