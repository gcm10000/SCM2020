using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Migrations
{
    public partial class NewMigrate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumptionProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(nullable: false),
                    Group = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: true),
                    Localization = table.Column<string>(nullable: true),
                    NumberLocalization = table.Column<long>(nullable: false),
                    Stock = table.Column<double>(nullable: false),
                    MininumStock = table.Column<double>(nullable: false),
                    MaximumStock = table.Column<double>(nullable: false),
                    Unity = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumptionProduct", x => x.Id);
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
                    Invoice = table.Column<string>(nullable: false),
                    MovingDate = table.Column<DateTime>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    SCMEmployeeId = table.Column<string>(nullable: false)
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
                    ClosingDate = table.Column<DateTime>(nullable: true),
                    SCMEmployeeId = table.Column<string>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: false),
                    Situation = table.Column<bool>(nullable: false),
                    RequestingSector = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitoring", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermanentProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InformationProduct = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateAdd = table.Column<DateTime>(nullable: false),
                    Patrimony = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermanentProduct", x => x.Id);
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
                name: "AuxiliarConsumption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    MaterialOutputId = table.Column<int>(nullable: true),
                    MaterialInputId = table.Column<int>(nullable: true),
                    SCMRegistration = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    MaterialInputByVendorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuxiliarConsumption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuxiliarConsumption_MaterialInputByVendor_MaterialInputByVendorId",
                        column: x => x.MaterialInputByVendorId,
                        principalTable: "MaterialInputByVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuxiliarConsumption_MaterialInput_MaterialInputId",
                        column: x => x.MaterialInputId,
                        principalTable: "MaterialInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuxiliarConsumption_MaterialOutput_MaterialOutputId",
                        column: x => x.MaterialOutputId,
                        principalTable: "MaterialOutput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuxiliarPermanent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    MaterialOutputId = table.Column<int>(nullable: true),
                    MaterialInputId = table.Column<int>(nullable: true),
                    SCMRegistration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuxiliarPermanent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuxiliarPermanent_MaterialInput_MaterialInputId",
                        column: x => x.MaterialInputId,
                        principalTable: "MaterialInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuxiliarPermanent_MaterialOutput_MaterialOutputId",
                        column: x => x.MaterialOutputId,
                        principalTable: "MaterialOutput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarConsumption_MaterialInputByVendorId",
                table: "AuxiliarConsumption",
                column: "MaterialInputByVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarConsumption_MaterialInputId",
                table: "AuxiliarConsumption",
                column: "MaterialInputId");

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarConsumption_MaterialOutputId",
                table: "AuxiliarConsumption",
                column: "MaterialOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanent_MaterialInputId",
                table: "AuxiliarPermanent",
                column: "MaterialInputId");

            migrationBuilder.CreateIndex(
                name: "IX_AuxiliarPermanent_MaterialOutputId",
                table: "AuxiliarPermanent",
                column: "MaterialOutputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuxiliarConsumption");

            migrationBuilder.DropTable(
                name: "AuxiliarPermanent");

            migrationBuilder.DropTable(
                name: "ConsumptionProduct");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Monitoring");

            migrationBuilder.DropTable(
                name: "PermanentProduct");

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
