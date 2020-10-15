using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class NewMigrate7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreMessage_SolicitationMessage_NotificationId",
                table: "StoreMessage");

            migrationBuilder.AddColumn<int>(
                name: "StoreMessageId",
                table: "SolicitationMessage",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlertStockMessageId",
                table: "Destination",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AlertStockMessage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertStockMessage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitationMessage_StoreMessageId",
                table: "SolicitationMessage",
                column: "StoreMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Destination_AlertStockMessageId",
                table: "Destination",
                column: "AlertStockMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destination_AlertStockMessage_AlertStockMessageId",
                table: "Destination",
                column: "AlertStockMessageId",
                principalTable: "AlertStockMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitationMessage_StoreMessage_StoreMessageId",
                table: "SolicitationMessage",
                column: "StoreMessageId",
                principalTable: "StoreMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMessage_AlertStockMessage_NotificationId",
                table: "StoreMessage",
                column: "NotificationId",
                principalTable: "AlertStockMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destination_AlertStockMessage_AlertStockMessageId",
                table: "Destination");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitationMessage_StoreMessage_StoreMessageId",
                table: "SolicitationMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMessage_AlertStockMessage_NotificationId",
                table: "StoreMessage");

            migrationBuilder.DropTable(
                name: "AlertStockMessage");

            migrationBuilder.DropIndex(
                name: "IX_SolicitationMessage_StoreMessageId",
                table: "SolicitationMessage");

            migrationBuilder.DropIndex(
                name: "IX_Destination_AlertStockMessageId",
                table: "Destination");

            migrationBuilder.DropColumn(
                name: "StoreMessageId",
                table: "SolicitationMessage");

            migrationBuilder.DropColumn(
                name: "AlertStockMessageId",
                table: "Destination");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMessage_SolicitationMessage_NotificationId",
                table: "StoreMessage",
                column: "NotificationId",
                principalTable: "SolicitationMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
