using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AboutProducts_Id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_AboutProducts_Id",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "AboutProducts");

            migrationBuilder.DropColumn(
                name: "VendorID",
                table: "AboutProducts");

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "AboutProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Vendor",
                table: "AboutProducts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "AboutProducts");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "AboutProducts");

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "AboutProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorID",
                table: "AboutProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AboutProducts_Id",
                table: "Groups",
                column: "Id",
                principalTable: "AboutProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_AboutProducts_Id",
                table: "Vendors",
                column: "Id",
                principalTable: "AboutProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
