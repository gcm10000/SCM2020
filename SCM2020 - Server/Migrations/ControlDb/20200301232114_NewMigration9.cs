using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutProducts_Groups_GroupID",
                table: "AboutProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AboutProducts_Vendors_VendorID",
                table: "AboutProducts");

            migrationBuilder.DropIndex(
                name: "IX_AboutProducts_GroupID",
                table: "AboutProducts");

            migrationBuilder.DropIndex(
                name: "IX_AboutProducts_VendorID",
                table: "AboutProducts");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AboutProducts_Id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_AboutProducts_Id",
                table: "Vendors");

            migrationBuilder.CreateIndex(
                name: "IX_AboutProducts_GroupID",
                table: "AboutProducts",
                column: "GroupID",
                unique: true,
                filter: "[GroupID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AboutProducts_VendorID",
                table: "AboutProducts",
                column: "VendorID",
                unique: true,
                filter: "[VendorID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutProducts_Groups_GroupID",
                table: "AboutProducts",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AboutProducts_Vendors_VendorID",
                table: "AboutProducts",
                column: "VendorID",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
