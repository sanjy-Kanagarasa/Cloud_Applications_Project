using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Review;
using WebApi.Models.SignalR;
using WebApi.Models.Users;

namespace WebApi.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        public DbSet<Flavour> Flavours { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItemFlavour> OrderItemFlavours { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public DbSet<DriverReview> DriverReview { get; set; }
        public DbSet<CustomerReview> CustomerReview { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // For Order
            modelBuilder.Entity<Order>()
                .HasOne(p => p.Customer)
                .WithMany(b => b.Orders)
                .HasForeignKey(p => p.CustomerID);

            modelBuilder.Entity<Order>()
               .HasOne(p => p.Driver)
               .WithMany(b => b.Orders)
               .HasForeignKey(p => p.CustomerID);


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

            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.Entity<ContactInformation>().ToTable("ContactInformation");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<CustomerReview>().ToTable("CustomerReviews");
            modelBuilder.Entity<DriverReview>().ToTable("DriverReviews");
            modelBuilder.Entity<DriverFlavour>().ToTable("DriverFlavours");
            modelBuilder.Entity<Driver>().ToTable("Drivers");
            modelBuilder.Entity<Flavour>().ToTable("Flavours");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<OrderItemFlavour>().ToTable("OrderItemFlavours");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Session>().ToTable("Sessions");
        }
    }
}
