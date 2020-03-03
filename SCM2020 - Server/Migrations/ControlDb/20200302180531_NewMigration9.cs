using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualProducts_MaterialInputByVendor_MaterialInputByVendorId",
                table: "IndividualProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_IndividualProducts_MaterialInput_MaterialInputId",
                table: "IndividualProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_IndividualProducts_MaterialOutput_MaterialOutputId",
                table: "IndividualProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IndividualProducts",
                table: "IndividualProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutProducts",
                table: "AboutProducts");

            migrationBuilder.RenameTable(
                name: "IndividualProducts",
                newName: "PermanentProduct");

            migrationBuilder.RenameTable(
                name: "AboutProducts",
                newName: "ConsumptionProduct");

            migrationBuilder.RenameIndex(
                name: "IX_IndividualProducts_MaterialOutputId",
                table: "PermanentProduct",
                newName: "IX_PermanentProduct_MaterialOutputId");

            migrationBuilder.RenameIndex(
                name: "IX_IndividualProducts_MaterialInputId",
                table: "PermanentProduct",
                newName: "IX_PermanentProduct_MaterialInputId");

            migrationBuilder.RenameIndex(
                name: "IX_IndividualProducts_MaterialInputByVendorId",
                table: "PermanentProduct",
                newName: "IX_PermanentProduct_MaterialInputByVendorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermanentProduct",
                table: "PermanentProduct",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsumptionProduct",
                table: "ConsumptionProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PermanentProduct_MaterialInputByVendor_MaterialInputByVendorId",
                table: "PermanentProduct",
                column: "MaterialInputByVendorId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermanentProduct_MaterialInput_MaterialInputId",
                table: "PermanentProduct",
                column: "MaterialInputId",
                principalTable: "MaterialInput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermanentProduct_MaterialOutput_MaterialOutputId",
                table: "PermanentProduct",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermanentProduct_MaterialInputByVendor_MaterialInputByVendorId",
                table: "PermanentProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PermanentProduct_MaterialInput_MaterialInputId",
                table: "PermanentProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PermanentProduct_MaterialOutput_MaterialOutputId",
                table: "PermanentProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermanentProduct",
                table: "PermanentProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsumptionProduct",
                table: "ConsumptionProduct");

            migrationBuilder.RenameTable(
                name: "PermanentProduct",
                newName: "IndividualProducts");

            migrationBuilder.RenameTable(
                name: "ConsumptionProduct",
                newName: "AboutProducts");

            migrationBuilder.RenameIndex(
                name: "IX_PermanentProduct_MaterialOutputId",
                table: "IndividualProducts",
                newName: "IX_IndividualProducts_MaterialOutputId");

            migrationBuilder.RenameIndex(
                name: "IX_PermanentProduct_MaterialInputId",
                table: "IndividualProducts",
                newName: "IX_IndividualProducts_MaterialInputId");

            migrationBuilder.RenameIndex(
                name: "IX_PermanentProduct_MaterialInputByVendorId",
                table: "IndividualProducts",
                newName: "IX_IndividualProducts_MaterialInputByVendorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndividualProducts",
                table: "IndividualProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutProducts",
                table: "AboutProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualProducts_MaterialInputByVendor_MaterialInputByVendorId",
                table: "IndividualProducts",
                column: "MaterialInputByVendorId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualProducts_MaterialInput_MaterialInputId",
                table: "IndividualProducts",
                column: "MaterialInputId",
                principalTable: "MaterialInput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualProducts_MaterialOutput_MaterialOutputId",
                table: "IndividualProducts",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
