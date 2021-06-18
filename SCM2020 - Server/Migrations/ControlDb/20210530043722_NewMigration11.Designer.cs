﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCM2020___Server.Context;

namespace SCM2020___Server.Migrations.ControlDb
{
    [DbContext(typeof(ControlDbContext))]
    [Migration("20210530043722_NewMigration11")]
    partial class NewMigration11
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ModelsLibraryCore.AlertStockMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Icon")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AlertStockMessage");
                });

            modelBuilder.Entity("ModelsLibraryCore.AuxiliarConsumption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MaterialInputByVendorId")
                        .HasColumnType("int");

                    b.Property<int?>("MaterialInputId")
                        .HasColumnType("int");

                    b.Property<int?>("MaterialOutputId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<string>("SCMEmployeeId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MaterialInputByVendorId");

                    b.HasIndex("MaterialInputId");

                    b.HasIndex("MaterialOutputId");

                    b.ToTable("AuxiliarConsumption");
                });

            modelBuilder.Entity("ModelsLibraryCore.AuxiliarPermanent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MaterialInputByVendorId")
                        .HasColumnType("int");

                    b.Property<int?>("MaterialInputId")
                        .HasColumnType("int");

                    b.Property<int?>("MaterialOutputId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("SCMEmployeeId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MaterialInputByVendorId");

                    b.HasIndex("MaterialInputId");

                    b.HasIndex("MaterialOutputId");

                    b.ToTable("AuxiliarPermanent");
                });

            modelBuilder.Entity("ModelsLibraryCore.Business", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Business");
                });

            modelBuilder.Entity("ModelsLibraryCore.ConsumptionProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Group")
                        .HasColumnType("int");

                    b.Property<string>("Localization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MaximumStock")
                        .HasColumnType("float");

                    b.Property<double>("MininumStock")
                        .HasColumnType("float");

                    b.Property<string>("Unity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("stock1")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ConsumptionProduct");
                });

            modelBuilder.Entity("ModelsLibraryCore.Destination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlertStockMessageId")
                        .HasColumnType("int");

                    b.Property<int?>("SolicitationMessageId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AlertStockMessageId");

                    b.HasIndex("SolicitationMessageId");

                    b.ToTable("Destination");
                });

            modelBuilder.Entity("ModelsLibraryCore.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BusinessId")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeesId")
                        .HasColumnType("int");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeesId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ModelsLibraryCore.EmployeeGroupSupport", b =>
                {
                    b.Property<int>("GroupEmployee1Id")
                        .HasColumnType("int");

                    b.Property<int>("GroupEmployee2Id")
                        .HasColumnType("int");

                    b.HasKey("GroupEmployee1Id", "GroupEmployee2Id");

                    b.HasIndex("GroupEmployee2Id");

                    b.ToTable("EmployeeGroupSupport");
                });

            modelBuilder.Entity("ModelsLibraryCore.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ModelsLibraryCore.GroupEmployees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PositionVertical")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GroupEmployees");
                });

            modelBuilder.Entity("ModelsLibraryCore.MaterialInput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Regarding")
                        .HasColumnType("int");

                    b.Property<string>("WorkOrder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MaterialInput");
                });

            modelBuilder.Entity("ModelsLibraryCore.MaterialInputByVendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Invoice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SCMEmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VendorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MaterialInputByVendor");
                });

            modelBuilder.Entity("ModelsLibraryCore.MaterialOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkOrder")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MaterialOutput");
                });

            modelBuilder.Entity("ModelsLibraryCore.Monitoring", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("ClosingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequestingSector")
                        .HasColumnType("int");

                    b.Property<string>("SCMEmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SectorId")
                        .HasColumnType("int");

                    b.Property<string>("ServiceLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Situation")
                        .HasColumnType("bit");

                    b.Property<string>("Work_Order")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Monitoring");
                });

            modelBuilder.Entity("ModelsLibraryCore.NumberSectors", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int?>("SectorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SectorId");

                    b.ToTable("NumberSectors");
                });

            modelBuilder.Entity("ModelsLibraryCore.PermanentProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdd")
                        .HasColumnType("datetime2");

                    b.Property<int>("InformationProduct")
                        .HasColumnType("int");

                    b.Property<string>("Patrimony")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("WorkOrder")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PermanentProduct");
                });

            modelBuilder.Entity("ModelsLibraryCore.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ConsumptionProductId")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConsumptionProductId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("ModelsLibraryCore.Sector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NameSector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sectors");
                });

            modelBuilder.Entity("ModelsLibraryCore.SolicitationMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Icon")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MonitoringId")
                        .HasColumnType("int");

                    b.Property<int?>("StoreMessageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MonitoringId");

                    b.HasIndex("StoreMessageId");

                    b.ToTable("SolicitationMessage");
                });

            modelBuilder.Entity("ModelsLibraryCore.StoreMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NotificationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId")
                        .IsUnique();

                    b.ToTable("StoreMessage");
                });

            modelBuilder.Entity("ModelsLibraryCore.UsersId", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("StoreMessageId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("StoreMessageId");

                    b.ToTable("UsersId");
                });

            modelBuilder.Entity("ModelsLibraryCore.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("ModelsLibraryCore.AuxiliarConsumption", b =>
                {
                    b.HasOne("ModelsLibraryCore.MaterialInputByVendor", "MaterialInputByVendor")
                        .WithMany("ConsumptionProducts")
                        .HasForeignKey("MaterialInputByVendorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ModelsLibraryCore.MaterialInput", "MaterialInput")
                        .WithMany("ConsumptionProducts")
                        .HasForeignKey("MaterialInputId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ModelsLibraryCore.MaterialOutput", "MaterialOutput")
                        .WithMany("ConsumptionProducts")
                        .HasForeignKey("MaterialOutputId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ModelsLibraryCore.AuxiliarPermanent", b =>
                {
                    b.HasOne("ModelsLibraryCore.MaterialInputByVendor", null)
                        .WithMany("PermanentProducts")
                        .HasForeignKey("MaterialInputByVendorId");

                    b.HasOne("ModelsLibraryCore.MaterialInput", "MaterialInput")
                        .WithMany("PermanentProducts")
                        .HasForeignKey("MaterialInputId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ModelsLibraryCore.MaterialOutput", "MaterialOutput")
                        .WithMany("PermanentProducts")
                        .HasForeignKey("MaterialOutputId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ModelsLibraryCore.Destination", b =>
                {
                    b.HasOne("ModelsLibraryCore.AlertStockMessage", null)
                        .WithMany("Destination")
                        .HasForeignKey("AlertStockMessageId");

                    b.HasOne("ModelsLibraryCore.SolicitationMessage", null)
                        .WithMany("Destination")
                        .HasForeignKey("SolicitationMessageId");
                });

            modelBuilder.Entity("ModelsLibraryCore.Employee", b =>
                {
                    b.HasOne("ModelsLibraryCore.GroupEmployees", "Employees")
                        .WithMany("Employees")
                        .HasForeignKey("EmployeesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ModelsLibraryCore.EmployeeGroupSupport", b =>
                {
                    b.HasOne("ModelsLibraryCore.GroupEmployees", "GroupEmployeesParent")
                        .WithMany("GroupEmployeesParent")
                        .HasForeignKey("GroupEmployee1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ModelsLibraryCore.GroupEmployees", "GroupEmployeesChild")
                        .WithMany("GroupEmployeesChild")
                        .HasForeignKey("GroupEmployee2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ModelsLibraryCore.NumberSectors", b =>
                {
                    b.HasOne("ModelsLibraryCore.Sector", "Sector")
                        .WithMany("NumberSectors")
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ModelsLibraryCore.Photo", b =>
                {
                    b.HasOne("ModelsLibraryCore.ConsumptionProduct", null)
                        .WithMany("Photos")
                        .HasForeignKey("ConsumptionProductId");
                });

            modelBuilder.Entity("ModelsLibraryCore.SolicitationMessage", b =>
                {
                    b.HasOne("ModelsLibraryCore.Monitoring", "Monitoring")
                        .WithMany()
                        .HasForeignKey("MonitoringId");

                    b.HasOne("ModelsLibraryCore.StoreMessage", "StoreMessage")
                        .WithMany()
                        .HasForeignKey("StoreMessageId");
                });

            modelBuilder.Entity("ModelsLibraryCore.StoreMessage", b =>
                {
                    b.HasOne("ModelsLibraryCore.AlertStockMessage", "Notification")
                        .WithOne("StoreMessage")
                        .HasForeignKey("ModelsLibraryCore.StoreMessage", "NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModelsLibraryCore.UsersId", b =>
                {
                    b.HasOne("ModelsLibraryCore.StoreMessage", "StoreMessage")
                        .WithMany("UsersId")
                        .HasForeignKey("StoreMessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
