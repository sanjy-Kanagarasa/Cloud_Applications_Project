using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Repositories;
using WebApi.Models.Review;
using WebApi.Models.SignalR;
using WebApi.Models.Users;

namespace WebApi.Data
{
    public class OrderDbInitializer
    {
        public static void Initialize(OrderContext context, UserContext userContext)
        {
            context.Database.EnsureCreated();

            if (context.Flavours.Any())
            {
                return;
            }

            var flavours = new List<Flavour>
            {
                new Flavour{ Name = "Vanilla", Price = 1.10 },
                new Flavour{ Name = "Chocolade", Price = 1.20 },
                new Flavour{ Name = "Pistache", Price = 1.30 },
                new Flavour{ Name = "Banana", Price = 1.40 },
                new Flavour{ Name = "Strawberry", Price = 1.50 }

            };

            foreach (var flavour in flavours)
            {
                context.Flavours.Add(flavour);
            }

            context.SaveChanges();

            var drivers = userContext.Drivers.ToList();
            foreach (var driver in drivers)
            {
                foreach (var flavour in flavours)
                {
                    userContext.DriverFlavours.Add(new DriverFlavour
                    {
                        DriverID = driver.DriverID,
                        FlavourID = flavours.Single(f => f.Name == flavour.Name).FlavourID,
                        Price = flavour.Price
                    });
                }
            }
            userContext.SaveChanges();

            var customers = userContext.Customers.ToList();
            context.DriverReview.Add(new DriverReview { CustomerID = customers[0].CustomerID, DriverID = drivers[0].DriverID, Rating = 5, Review = "Very good" });
            context.SaveChanges();

            context.Sessions.Add(new Session { ConnectionID = "1315", Email = "test@uber.be" });
            context.SaveChanges();



            //context.CustomerReview.Add(new CustomerReview { Customer = new Customer { FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now, Location = new Location { latitude = (float)(51.2001783), longitude = (float)4.4327702 }, ContactInformation = new ContactInformation { Email = "chingiz-customer79@uber.be", PhoneNumber = "4443332221111", Address = new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060" } } }, Driver = drivers[0], Rating = 5, Review = "Very good" });
            //context.SaveChanges();

        }
    }
}
