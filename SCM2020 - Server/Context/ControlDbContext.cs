﻿using Microsoft.EntityFrameworkCore;
using ModelsLibraryCore;
using System.Threading;

namespace SCM2020___Server.Context
{
    public class ControlDbContext : DbContext
    {
        public DbSet<ConsumptionProduct> ConsumptionProduct { get; set; }
        public DbSet<PermanentProduct> PermanentProduct { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<MaterialInputByVendor> MaterialInputByVendor { get; set; }
        public DbSet<MaterialInput> MaterialInput { get; set; }
        public DbSet<MaterialOutput> MaterialOutput { get; set; }
        public DbSet<Monitoring> Monitoring { get; set; }
        public DbSet<AuxiliarConsumption> AuxiliarConsumption { get; set; }
        public DbSet<AuxiliarPermanent> AuxiliarPermanent { get; set; }
        public ControlDbContext(DbContextOptions<ControlDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AboutProduct>()
            //    .HasOne(s => s.Vendor)
            //    .WithOne()
            //    .HasForeignKey<Vendor>(e => e.Id)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<AboutProduct>()
            //    .HasOne(s => s.Group)
            //    .WithOne()
            //    .HasForeignKey<Group>(e => e.Id)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuxiliarConsumption>()
                .HasOne(b => b.MaterialInputByVendor)
                .WithMany(a => a.AuxiliarConsumptions)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuxiliarConsumption>()
                .HasOne(b => b.MaterialOutput)
                .WithMany(a => a.ConsumptionProducts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuxiliarPermanent>()
                .HasOne(b => b.MaterialOutput)
                .WithMany(a => a.PermanentProducts)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<AuxiliarConsumption>()
                .HasOne(b => b.MaterialInput)
                .WithMany(a => a.ConsumptionProducts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuxiliarPermanent>()
                .HasOne(b => b.MaterialInput)
                .WithMany(a => a.PermanentProducts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Monitoring>()
                .HasOne(b => b.MaterialOutput)
                .WithOne(a => a.Monitoring)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Monitoring>()
                .HasOne(b => b.MaterialInput)
                .WithOne(a => a.Monitoring)
                .OnDelete(DeleteBehavior.Cascade);

            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

        }

    }
}
