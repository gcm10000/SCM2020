using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermanentId",
                table: "PermanentOutput");

            migrationBuilder.DropColumn(
                name: "ConsumperId",
                table: "ConsumptionOutput");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "PermanentOutput",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "PermanentOutput",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "SCMEmployeeId",
                table: "Monitoring",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Monitoring",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ConsumptionOutput",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ConsumptionOutput",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "PermanentOutput");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PermanentOutput");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ConsumptionOutput");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ConsumptionOutput");

            migrationBuilder.AddColumn<int>(
                name: "PermanentId",
                table: "PermanentOutput",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SCMEmployeeId",
                table: "Monitoring",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Monitoring",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "ConsumperId",
                table: "ConsumptionOutput",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
