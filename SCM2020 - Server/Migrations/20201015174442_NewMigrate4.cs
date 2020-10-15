using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class NewMigrate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationId",
                table: "StoreMessage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SolicitationMessage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitationMessage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreMessage_NotificationId",
                table: "StoreMessage",
                column: "NotificationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMessage_SolicitationMessage_NotificationId",
                table: "StoreMessage",
                column: "NotificationId",
                principalTable: "SolicitationMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreMessage_SolicitationMessage_NotificationId",
                table: "StoreMessage");

            migrationBuilder.DropTable(
                name: "SolicitationMessage");

            migrationBuilder.DropIndex(
                name: "IX_StoreMessage_NotificationId",
                table: "StoreMessage");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "StoreMessage");
        }
    }
}
