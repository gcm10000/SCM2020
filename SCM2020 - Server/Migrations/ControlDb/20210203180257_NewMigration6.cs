using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberSector",
                table: "Sectors");

            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "NumberSectors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NumberSectors_SectorId",
                table: "NumberSectors",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_NumberSectors_Sectors_SectorId",
                table: "NumberSectors",
                column: "SectorId",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NumberSectors_Sectors_SectorId",
                table: "NumberSectors");

            migrationBuilder.DropIndex(
                name: "IX_NumberSectors_SectorId",
                table: "NumberSectors");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "NumberSectors");

            migrationBuilder.AddColumn<int>(
                name: "NumberSector",
                table: "Sectors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
