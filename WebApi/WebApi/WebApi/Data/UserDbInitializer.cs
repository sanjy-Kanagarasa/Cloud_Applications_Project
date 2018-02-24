using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Users;

namespace WebApi.Data
{
    public class UserDbInitializer
    {

        public static void Initialize(UserContext context)
        {
            context.Database.EnsureCreated();

            if (context.Customers.Any())
            {
                return;
            }

            AddCustomersToDatabase(context);
            AddDriversToDatabse(context);

            //var flavours = new List<Flavour>
            //{
            //    new Flavour{ Name = "Vanilla", Price = 1.10 },
            //    new Flavour{ Name = "Chocolade", Price = 1.20 },
            //    new Flavour{ Name = "Pistache", Price = 1.30 },
            //    new Flavour{ Name = "Banana", Price = 1.40 },
            //    new Flavour{ Name = "Strawberry", Price = 1.50 }

            //};

            //foreach (var flavour in flavours)
            //{
            //    context.Flavours.Add(flavour);
            //}

            //context.SaveChanges();

            //var driversFromDB = context.Drivers.ToList();
            //foreach (var driver in driversFromDB)
            //{
            //    foreach (var flavour in flavours)
            //    {
            //        context.DriverFlavours.Add(new DriverFlavour
            //        {
            //            DriverID = driver.DriverID,
            //            FlavourID = flavours.Single(f => f.Name == flavour.Name).FlavourID,
            //            Price = flavour.Price
            //        });
            //    }
            //}
            //context.SaveChanges();



        }

        private static void AddDriversToDatabse(UserContext context)
        {
            var drivers = new List<Driver>
            {
                new Driver{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.2301299), longitude = (float) ( 4.4161723) }, ContactInformation = new ContactInformation { Email = "sanjy-driver@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.2469382), longitude =  (float) (4.45203781)}, ContactInformation = new ContactInformation { Email = "chingiz-driver@uber.be", PhoneNumber = "4443332221111", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Stijn", LastName = "Pittomvils", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.25596345), longitude =  (float) (4.42354202)}, ContactInformation = new ContactInformation { Email = "stijn-driver@uber.be", PhoneNumber = "99999999999", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}, IsApproved = true },
                new Driver{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.22931236), longitude =  (float) (4.50971604)}, ContactInformation = new ContactInformation { Email = "sanjy-driver1@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.20221559), longitude =  (float) (4.5227623)}, ContactInformation = new ContactInformation { Email = "chingiz-driver1@uber.be", PhoneNumber = "4443332221111", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Stijn", LastName = "Pittomvils", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.19597678), longitude =  (float) (4.48516846)}, ContactInformation = new ContactInformation { Email = "stijn-driver1@uber.be", PhoneNumber = "99999999999", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}, IsApproved = true },
                new Driver{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.19092056), longitude =  (float) (4.45014954)}, ContactInformation = new ContactInformation { Email = "sanjy-driver2@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.19565406), longitude =  (float) (4.41839218)}, ContactInformation = new ContactInformation { Email = "chingiz-driver2@uber.be", PhoneNumber = "4443332221111", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Stijn", LastName = "Pittomvils", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.22178708), longitude =  (float) (4.46165085)}, ContactInformation = new ContactInformation { Email = "stijn-driver2@uber.be", PhoneNumber = "99999999999", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}, IsApproved = true },

            };

            foreach (var driver in drivers)
            {
                context.Drivers.Add(driver);
            }

            context.SaveChanges();
        }

        private static void AddCustomersToDatabase(UserContext context)
        {
            var customers = new List<Customer>
            {
                new Customer{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now, Location = new Location {latitude = (float) (51.2304895), longitude = (float) (4.4774069)}, ContactInformation = new ContactInformation { Email = "sanjy-customer@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Customer{ FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now, Location = new Location {latitude = (float) (51.2001783), longitude = (float) 4.4327702}, ContactInformation = new ContactInformation { Email = "chingiz-customer@uber.be", PhoneNumber = "4443332221111", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Customer{ FirstName = "Stijn", LastName = "Pittomvils", Password = "test123", RegistrationDate = DateTime.Now, Location = new Location {latitude = (float) (51.1892618), longitude = (float) 4.7385712}, ContactInformation = new ContactInformation { Email = "stijn-customer@uber.be", PhoneNumber = "99999999999", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},

            };

            foreach (var customer in customers)
            {
                context.Customers.Add(customer);
            }

            context.SaveChanges();
        }
    }
}
