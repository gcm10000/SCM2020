﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCM2020___Server.Context;

namespace SCM2020___Server.Migrations.ControlDb
{
    [DbContext(typeof(ControlDbContext))]
    partial class ControlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SCM2020___Server.Models.ConsumptionOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MaterialOutputId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaterialOutputId");

                    b.ToTable("ConsumptionOutput");
                });

            modelBuilder.Entity("SCM2020___Server.Models.ConsumptionProduct", b =>
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

            modelBuilder.Entity("SCM2020___Server.Models.Group", b =>
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

            modelBuilder.Entity("SCM2020___Server.Models.MaterialInput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DocDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Regarding")
                        .HasColumnType("int");

                    b.Property<int>("SCMEmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("WorkOrder")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MaterialInput");
                });

            modelBuilder.Entity("SCM2020___Server.Models.MaterialInputByVendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Invoice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("MaterialInputByVendor");
                });

            modelBuilder.Entity("SCM2020___Server.Models.MaterialOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmployeeRegistration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequestingSector")
                        .HasColumnType("int");

                    b.Property<string>("SCMRegistration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkOrder")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MaterialOutput");
                });

            modelBuilder.Entity("SCM2020___Server.Models.Monitoring", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ClosingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MovingDate")
                        .HasColumnType("datetime2");

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

            modelBuilder.Entity("SCM2020___Server.Models.PermanentOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MaterialOutputId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaterialOutputId");

                    b.ToTable("PermanentOutput");
                });

            modelBuilder.Entity("SCM2020___Server.Models.PermanentProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdd")
                        .HasColumnType("datetime2");

                    b.Property<int>("InformationProduct")
                        .HasColumnType("int");

                    b.Property<int?>("MaterialInputByVendorId")
                        .HasColumnType("int");

                    b.Property<int?>("MaterialInputId")
                        .HasColumnType("int");

                    b.Property<string>("Patrimony")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaterialInputByVendorId");

                    b.HasIndex("MaterialInputId");

                    b.ToTable("PermanentProduct");
                });

            modelBuilder.Entity("SCM2020___Server.Models.Sector", b =>
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

            modelBuilder.Entity("SCM2020___Server.Models.Vendor", b =>
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

            modelBuilder.Entity("SCM2020___Server.Models.ConsumptionOutput", b =>
                {
                    b.HasOne("SCM2020___Server.Models.MaterialOutput", null)
                        .WithMany("ConsumptionProducts")
                        .HasForeignKey("MaterialOutputId");
                });

            modelBuilder.Entity("SCM2020___Server.Models.PermanentOutput", b =>
                {
                    b.HasOne("SCM2020___Server.Models.MaterialOutput", null)
                        .WithMany("PermanentProducts")
                        .HasForeignKey("MaterialOutputId");
                });

            modelBuilder.Entity("SCM2020___Server.Models.PermanentProduct", b =>
                {
                    b.HasOne("SCM2020___Server.Models.MaterialInputByVendor", null)
                        .WithMany("Products")
                        .HasForeignKey("MaterialInputByVendorId");

                    b.HasOne("SCM2020___Server.Models.MaterialInput", null)
                        .WithMany("Products")
                        .HasForeignKey("MaterialInputId");
                });
#pragma warning restore 612, 618
        }
    }
}
