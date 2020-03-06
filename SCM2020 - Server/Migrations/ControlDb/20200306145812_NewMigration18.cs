using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermanentProduct_MaterialInputByVendor_MaterialInputByVendorId",
                table: "PermanentProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PermanentProduct_MaterialInput_MaterialInputId",
                table: "PermanentProduct");

            migrationBuilder.DropIndex(
                name: "IX_PermanentProduct_MaterialInputByVendorId",
                table: "PermanentProduct");

            migrationBuilder.DropIndex(
                name: "IX_PermanentProduct_MaterialInputId",
                table: "PermanentProduct");

            migrationBuilder.DropColumn(
                name: "MaterialInputByVendorId",
                table: "PermanentProduct");

            migrationBuilder.DropColumn(
                name: "MaterialInputId",
                table: "PermanentProduct");

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "PermanentOutput",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCMEmployeeId",
                table: "MaterialInputByVendor",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "MaterialInputByVendor",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "SCMEmployeeId",
                table: "MaterialInput",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "MaterialInput",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "ConsumptionProduct",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputId",
                table: "ConsumptionProduct",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermanentOutput_MaterialInputByVendorId",
                table: "PermanentOutput",
                column: "MaterialInputByVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionProduct_MaterialInputByVendorId",
                table: "ConsumptionProduct",
                column: "MaterialInputByVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionProduct_MaterialInputId",
                table: "ConsumptionProduct",
                column: "MaterialInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumptionProduct_MaterialInputByVendor_MaterialInputByVendorId",
                table: "ConsumptionProduct",
                column: "MaterialInputByVendorId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumptionProduct_MaterialInput_MaterialInputId",
                table: "ConsumptionProduct",
                column: "MaterialInputId",
                principalTable: "MaterialInput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermanentOutput_MaterialInputByVendor_MaterialInputByVendorId",
                table: "PermanentOutput",
                column: "MaterialInputByVendorId",
                principalTable: "MaterialInputByVendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionProduct_MaterialInputByVendor_MaterialInputByVendorId",
                table: "ConsumptionProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionProduct_MaterialInput_MaterialInputId",
                table: "ConsumptionProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PermanentOutput_MaterialInputByVendor_MaterialInputByVendorId",
                table: "PermanentOutput");

            migrationBuilder.DropIndex(
                name: "IX_PermanentOutput_MaterialInputByVendorId",
                table: "PermanentOutput");

            migrationBuilder.DropIndex(
                name: "IX_ConsumptionProduct_MaterialInputByVendorId",
                table: "ConsumptionProduct");

            migrationBuilder.DropIndex(
                name: "IX_ConsumptionProduct_MaterialInputId",
                table: "ConsumptionProduct");

            migrationBuilder.DropColumn(
                name: "MaterialInputByVendorId",
                table: "PermanentOutput");

            migrationBuilder.DropColumn(
                name: "SCMEmployeeId",
                table: "MaterialInputByVendor");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "MaterialInputByVendor");

            migrationBuilder.DropColumn(
                name: "MaterialInputByVendorId",
                table: "ConsumptionProduct");

            migrationBuilder.DropColumn(
                name: "MaterialInputId",
                table: "ConsumptionProduct");

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "PermanentProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputId",
                table: "PermanentProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SCMEmployeeId",
                table: "MaterialInput",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "MaterialInput",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_PermanentProduct_MaterialInputByVendorId",
                table: "PermanentProduct",
                column: "MaterialInputByVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PermanentProduct_MaterialInputId",
                table: "PermanentProduct",
                column: "MaterialInputId");

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
        }
    }
}
