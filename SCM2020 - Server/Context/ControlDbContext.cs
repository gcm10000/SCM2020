using Microsoft.EntityFrameworkCore;
using SCM2020___Server.Models;

namespace SCM2020___Server.Context
{
    public class ControlDbContext : DbContext
    {
        public DbSet<AboutProduct> AboutProducts { get; set; }
        public DbSet<SpecificProduct> IndividualProducts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<MaterialInputByVendor> MaterialInputByVendor { get; set; }
        public DbSet<MaterialInput> MaterialInput { get; set; }
        public DbSet<MaterialOutput> MaterialOutput { get; set; }
        public DbSet<Monitoring> Monitoring { get; set; }
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

        }
    }
}
