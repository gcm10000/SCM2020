using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermanentProduct_MaterialOutput_MaterialOutputId",
                table: "PermanentProduct");

            migrationBuilder.DropIndex(
                name: "IX_PermanentProduct_MaterialOutputId",
                table: "PermanentProduct");

            migrationBuilder.DropColumn(
                name: "MaterialOutputId",
                table: "PermanentProduct");

            migrationBuilder.CreateTable(
                name: "ConsumpterOutput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsumperId = table.Column<int>(nullable: false),
                    MaterialOutputId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumpterOutput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumpterOutput_MaterialOutput_MaterialOutputId",
                        column: x => x.MaterialOutputId,
                        principalTable: "MaterialOutput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermanentOutput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermanentId = table.Column<int>(nullable: false),
                    MaterialOutputId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermanentOutput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermanentOutput_MaterialOutput_MaterialOutputId",
                        column: x => x.MaterialOutputId,
                        principalTable: "MaterialOutput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumpterOutput_MaterialOutputId",
                table: "ConsumpterOutput",
                column: "MaterialOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_PermanentOutput_MaterialOutputId",
                table: "PermanentOutput",
                column: "MaterialOutputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumpterOutput");

            migrationBuilder.DropTable(
                name: "PermanentOutput");

            migrationBuilder.AddColumn<int>(
                name: "MaterialOutputId",
                table: "PermanentProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermanentProduct_MaterialOutputId",
                table: "PermanentProduct",
                column: "MaterialOutputId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermanentProduct_MaterialOutput_MaterialOutputId",
                table: "PermanentProduct",
                column: "MaterialOutputId",
                principalTable: "MaterialOutput",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
