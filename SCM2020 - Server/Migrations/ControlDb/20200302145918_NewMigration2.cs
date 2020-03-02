using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations.ControlDb
{
    public partial class NewMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(nullable: false),
                    Group = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: true),
                    Block = table.Column<string>(nullable: true),
                    Localization = table.Column<int>(nullable: false),
                    Drawer = table.Column<long>(nullable: false),
                    Vendor = table.Column<int>(nullable: true),
                    Stock = table.Column<double>(nullable: false),
                    MininumStock = table.Column<double>(nullable: false),
                    MaximumStock = table.Column<double>(nullable: false),
                    Unity = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PJERJRegistration = table.Column<string>(nullable: true),
                    CPFRegistration = table.Column<string>(nullable: true),
                    IsPJERJRegistration = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Occupation = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: false),
                    AspNetUsersId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Regarding = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    SCMEmployeeId = table.Column<int>(nullable: false),
                    MovingDate = table.Column<DateTime>(nullable: false),
                    DocDate = table.Column<DateTime>(nullable: false),
                    WorkOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialInput", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialInputByVendor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Invoice = table.Column<string>(nullable: true),
                    MovingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialInputByVendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialOutput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovingDate = table.Column<DateTime>(nullable: false),
                    SCMRegistration = table.Column<string>(nullable: true),
                    EmployeeRegistration = table.Column<string>(nullable: true),
                    RequestingSector = table.Column<int>(nullable: false),
                    WorkOrder = table.Column<string>(nullable: true),
                    ServiceLocation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialOutput", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Monitoring",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Work_Order = table.Column<string>(nullable: false),
                    MovingDate = table.Column<DateTime>(nullable: false),
                    ClosingDate = table.Column<DateTime>(nullable: false),
                    SCMEmployeeId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    Situation = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitoring", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberSector = table.Column<int>(nullable: false),
                    NameSector = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndividualProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InformationProduct = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateAdd = table.Column<DateTime>(nullable: false),
                    Patrimony = table.Column<string>(nullable: true),
                    MaterialInputByVendorId = table.Column<int>(nullable: true),
                    MaterialInputId = table.Column<int>(nullable: true),
                    MaterialOutputId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualProducts_MaterialInputByVendor_MaterialInputByVendorId",
                        column: x => x.MaterialInputByVendorId,
                        principalTable: "MaterialInputByVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndividualProducts_MaterialInput_MaterialInputId",
                        column: x => x.MaterialInputId,
                        principalTable: "MaterialInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndividualProducts_MaterialOutput_MaterialOutputId",
                        column: x => x.MaterialOutputId,
                        principalTable: "MaterialOutput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndividualProducts_MaterialInputByVendorId",
                table: "IndividualProducts",
                column: "MaterialInputByVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualProducts_MaterialInputId",
                table: "IndividualProducts",
                column: "MaterialInputId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualProducts_MaterialOutputId",
                table: "IndividualProducts",
                column: "MaterialOutputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutProducts");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "IndividualProducts");

            migrationBuilder.DropTable(
                name: "Monitoring");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "MaterialInputByVendor");

            migrationBuilder.DropTable(
                name: "MaterialInput");

            migrationBuilder.DropTable(
                name: "MaterialOutput");
        }
    }
}
