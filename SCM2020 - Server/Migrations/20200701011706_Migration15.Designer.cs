﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCM2020___Server.Context;

namespace SCM2020___Server.Migrations
{
    [DbContext(typeof(ControlDbContext))]
    [Migration("20200701011706_Migration15")]
    partial class Migration15
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<int?>("MaterialInputId")
                        .HasColumnType("int");

                    b.Property<int?>("MaterialOutputId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("SCMEmployeeId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MaterialInputId");

                    b.HasIndex("MaterialOutputId");

                    b.ToTable("AuxiliarPermanent");
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

                    b.Property<long>("NumberLocalization")
                        .HasColumnType("bigint");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Stock")
                        .HasColumnType("float");

                    b.Property<string>("Unity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ConsumptionProduct");
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

            modelBuilder.Entity("ModelsLibraryCore.MaterialInput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DocDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Regarding")
                        .HasColumnType("int");

                    b.Property<string>("WorkOrder")
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

                    b.Property<string>("ServiceLocation")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<bool>("Situation")
                        .HasColumnType("bit");

                    b.Property<string>("Work_Order")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Monitoring");
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

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("Patrimony")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PermanentProduct");
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

                    b.Property<int>("NumberSector")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Sectors");
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
                        .WithMany("AuxiliarConsumptions")
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
                    b.HasOne("ModelsLibraryCore.MaterialInput", "MaterialInput")
                        .WithMany("PermanentProducts")
                        .HasForeignKey("MaterialInputId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ModelsLibraryCore.MaterialOutput", "MaterialOutput")
                        .WithMany("PermanentProducts")
                        .HasForeignKey("MaterialOutputId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
