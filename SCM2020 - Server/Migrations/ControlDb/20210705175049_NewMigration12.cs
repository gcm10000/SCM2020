using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "AuxiliarPermanentInputByVendor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    MaterialOutputId = table.Column<int>(nullable: true),
                    MaterialInputId = table.Column<int>(nullable: true),
                    SCMEmployeeId = table.Column<string>(nullable: true),
                    MaterialInputByVendorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuxiliarPermanentInputByVendor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuxiliarPermanentInputByVendor_MaterialInputByVendor_MaterialInputByVendorId",
                        column: x => x.MaterialInputByVendorId,
                        principalTable: "MaterialInputByVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuxiliarPermanentInputByVendor_MaterialInput_MaterialInputId",
                        column: x => x.MaterialInputId,
                        principalTable: "MaterialInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuxiliarPermanentInputByVendor_MaterialOutput_MaterialOutputId",
                        column: x => x.MaterialOutputId,
                        principalTable: "MaterialOutput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanentInputByVendor_MaterialInputByVendorId",
                table: "AuxiliarPermanentInputByVendor",
                column: "MaterialInputByVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanentInputByVendor_MaterialInputId",
                table: "AuxiliarPermanentInputByVendor",
                column: "MaterialInputId");

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanentInputByVendor_MaterialOutputId",
                table: "AuxiliarPermanentInputByVendor",
                column: "MaterialOutputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuxiliarPermanentInputByVendor");

            migrationBuilder.AddColumn<int>(
                name: "MaterialInputByVendorId",
                table: "AuxiliarPermanent",
                type: "int",
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
    }
}
