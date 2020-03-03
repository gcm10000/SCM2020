using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumpterOutput_MaterialOutput_MaterialOutputId",
                table: "ConsumpterOutput");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsumpterOutput",
                table: "ConsumpterOutput");

            migrationBuilder.RenameTable(
                name: "ConsumpterOutput",
                newName: "ConsumptionOutput");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumpterOutput_MaterialOutputId",
                table: "ConsumptionOutput",
                newName: "IX_ConsumptionOutput_MaterialOutputId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionOutput_MaterialOutput_MaterialOutputId",
                table: "ConsumptionOutput");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsumptionOutput",
                table: "ConsumptionOutput");

            migrationBuilder.RenameTable(
                name: "ConsumptionOutput",
                newName: "ConsumpterOutput");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumptionOutput_MaterialOutputId",
                table: "ConsumpterOutput",
                newName: "IX_ConsumpterOutput_MaterialOutputId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsumpterOutput",
                table: "ConsumpterOutput",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumpterOutput_MaterialOutput_MaterialOutputId",
                table: "ConsumpterOutput",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
