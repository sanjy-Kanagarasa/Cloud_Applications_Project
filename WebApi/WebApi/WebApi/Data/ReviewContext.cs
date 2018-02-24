using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Review;
using WebApi.Models.SignalR;
using WebApi.Models.Users;

namespace WebApi.Data
{
    public class ReviewContext : DbContext
    {
        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options) { }

        public DbSet<DriverReview> DriverReview { get; set; }
        public DbSet<CustomerReview> CustomerReview { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
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


            modelBuilder.Entity<DriverReview>()
                .HasOne(m => m.Customer);
            modelBuilder.Entity<CustomerReview>()
                .HasOne(m => m.Driver);

            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.Entity<ContactInformation>().ToTable("ContactInformation");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<CustomerReview>().ToTable("CustomerReviews");
            modelBuilder.Entity<DriverFlavour>().ToTable("DriverFlavours");
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
