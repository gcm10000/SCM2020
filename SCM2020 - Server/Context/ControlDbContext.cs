using Microsoft.EntityFrameworkCore;
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
        public DbSet<StoreMessage> StoreMessage { get; set; }
        public DbSet<UsersId> UsersId { get; set; }
        public DbSet<Business> Business { get; set; }
        public ControlDbContext(DbContextOptions<ControlDbContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}
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
                .WithMany(a => a.ConsumptionProducts)
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

            modelBuilder.Entity<UsersId>()
                .HasOne(b => b.StoreMessage)
                .WithMany(a => a.UsersId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SolicitationMessage>()
                .HasOne(b => b.StoreMessage)
                .WithOne(a => a.Notification as SolicitationMessage)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey<StoreMessage>(f => f.NotificationId);

            modelBuilder.Entity<AlertStockMessage>()
                .HasOne(b => b.StoreMessage)
                .WithOne(a => a.Notification as AlertStockMessage)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey<StoreMessage>(f => f.NotificationId);
            
            modelBuilder.Entity<Business>()
                .HasOne(b => b.ApplicationUser)
                .WithOne(a => a.Business)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey<ApplicationUser>(f => f.BusinessId);
            
            modelBuilder.Entity<Sector>()
                .HasOne(b => b.ApplicationUser)
                .WithOne(a => a.Sector)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey<ApplicationUser>(f => f.SectorId);

            modelBuilder.Entity<Sector>()
                .HasOne(b => b.Monitoring)
                .WithOne(a => a.Sector)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey<ApplicationUser>(f => f.SectorId);


            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

        }

    }
}
