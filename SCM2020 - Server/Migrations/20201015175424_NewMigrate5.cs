using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class NewMigrate5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Icon",
                table: "SolicitationMessage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "SolicitationMessage",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MonitoringId",
                table: "SolicitationMessage",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Destination",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    SolicitationMessageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destination_SolicitationMessage_SolicitationMessageId",
                        column: x => x.SolicitationMessageId,
                        principalTable: "SolicitationMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitationMessage_MonitoringId",
                table: "SolicitationMessage",
                column: "MonitoringId");

            migrationBuilder.CreateIndex(
                name: "IX_Destination_SolicitationMessageId",
                table: "Destination",
                column: "SolicitationMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitationMessage_Monitoring_MonitoringId",
                table: "SolicitationMessage",
                column: "MonitoringId",
                principalTable: "Monitoring",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitationMessage_Monitoring_MonitoringId",
                table: "SolicitationMessage");

            migrationBuilder.DropTable(
                name: "Destination");

            migrationBuilder.DropIndex(
                name: "IX_SolicitationMessage_MonitoringId",
                table: "SolicitationMessage");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "SolicitationMessage");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "SolicitationMessage");

            migrationBuilder.DropColumn(
                name: "MonitoringId",
                table: "SolicitationMessage");
        }
    }
}
