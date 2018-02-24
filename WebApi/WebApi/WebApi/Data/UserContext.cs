using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Review;
using WebApi.Models.SignalR;

namespace WebApi.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<ContactInformation> ContactInformation { get; set; }        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverFlavour> DriverFlavours { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(a => a.ContactInformation);

            modelBuilder.Entity<Customer>()
                .HasOne(a => a.Location);

            modelBuilder.Entity<Driver>()
               .HasOne(a => a.ContactInformation);

            modelBuilder.Entity<Driver>()
                .HasOne(a => a.Location);

            modelBuilder.Entity<ContactInformation>()
                .HasOne(a => a.Address);

            modelBuilder
                .Entity<ContactInformation>()
                 .HasIndex(e => e.Email)
                 .IsUnique(true);

            // For OrderItemFlavour
            modelBuilder.Entity<OrderItemFlavour>()
                .HasKey(k => new { k.OrderItemID, k.FlavourID });

            modelBuilder.Entity<OrderItemFlavour>()
                .HasOne(m => m.Flavour)
                .WithMany(mr => mr.OrderItemFlavours)
                .HasForeignKey(pt => pt.FlavourID);

            modelBuilder.Entity<OrderItemFlavour>()
                .HasOne(m => m.OrderItem)
                .WithMany(mr => mr.OrderItemFlavours)
                .HasForeignKey(pt => pt.OrderItemID);

            //for DriverFlavour

            modelBuilder.Entity<DriverFlavour>()
                .HasKey(k => new { k.FlavourID, k.DriverID });

            modelBuilder.Entity<DriverFlavour>()
                .HasOne(m => m.Driver)
                .WithMany(mr => mr.DriverFlavours)
                .HasForeignKey(pt => pt.DriverID);

            modelBuilder.Entity<DriverFlavour>()
                .HasOne(m => m.Flavour)
                .WithMany(mr => mr.DriverFlavours)
                .HasForeignKey(pt => pt.FlavourID);

        }
    }

    
}
