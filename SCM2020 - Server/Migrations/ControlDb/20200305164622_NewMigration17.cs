using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "PermanentOutput",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ConsumptionOutput",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "PermanentOutput");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ConsumptionOutput");
        }
    }
}
