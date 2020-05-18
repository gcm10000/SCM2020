using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigrate18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosingDate",
                table: "Monitoring",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosingDate",
                table: "Monitoring",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
