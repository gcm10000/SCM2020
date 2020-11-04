using Microsoft.EntityFrameworkCore;
using ModelsLibraryCore;
using System.Collections.Generic;
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
        public DbSet<Employee> Employees { get; set; }
        public DbSet<GroupEmployees> GroupEmployees { get; set; }
        public DbSet<EmployeeGroupSupport> EmployeeGroupSupport { get; set; }
        //public DbSet<TreeNode<KeyValuePair<CompanyPosition, List<Employee>>>> TreeView { get; set; }

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

            modelBuilder.Entity<Employee>()
                .HasOne(b => b.Employees)
                .WithMany(a => a.Employees)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeGroupSupport>()
                .HasKey(b => new { b.GroupEmployee1Id, b.GroupEmployee2Id });

            modelBuilder.Entity<EmployeeGroupSupport>()
                .HasOne(b => b.GroupEmployeesParent)
                .WithMany(a => a.GroupEmployees1)
                .HasForeignKey(b => b.GroupEmployee1Id)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<EmployeeGroupSupport>()
                .HasOne(b => b.GroupEmployeesChild)
                .WithMany(a => a.GroupEmployees2)
                .HasForeignKey(b => b.GroupEmployee2Id)
                .OnDelete(DeleteBehavior.Restrict);




            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

        }

    }
}
