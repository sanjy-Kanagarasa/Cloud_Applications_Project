using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.Orders;
using WebApi.Models.Resources;
using WebApi.Models.Users;

namespace WebApi.Models.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly OrderContext orderContext;
        private readonly IUsersRepository usersRepo;
        private readonly UserContext userContext;
        private readonly IMapper mapper;

        public DriverRepository(OrderContext orderContext, IUsersRepository usersRepo, 
            UserContext userContext, IMapper mapper)
        {
            this.orderContext = orderContext;
            this.usersRepo = usersRepo;
            this.userContext = userContext;
            this.mapper = mapper;
        }

        public async Task<Driver> GetFlavoursPrice(string email)
        {
            var driver = await usersRepo.GetDriverByEmail(email);
            if (driver != null)
            {
                return await userContext.Drivers
                    .Include(r => r.DriverFlavours)
                        .ThenInclude(df => df.Flavour)
                    .Include(r => r.ContactInformation)
                    .Include(r => r.Location)
                    .SingleOrDefaultAsync(d => d.DriverID == driver.DriverID);

            }
            return null;
        }

        public async Task<List<OrderTotalPriceResource>> CalculatePriceForAllDrivers(ShoppingCart shoppingcart, Order currentOrder)
        {
            var drivers = await usersRepo.GetDrivers();
            List<OrderTotalPriceResource> listPriceResources = new List<OrderTotalPriceResource>();
            

            foreach (var driver in drivers)
            {
                double totPrice = 0;
                var driverWithFlavours = await GetFlavoursPrice(driver.ContactInformation.Email);
                var resultDriver = mapper.Map<Driver, DriverResource>(driverWithFlavours);
                foreach (var order in shoppingcart.Cart)
                {
                    double subPrice = 0;
                    foreach (var item in order.IceCream)
                    {
                        foreach (var driverFlavour in resultDriver.Flavours)
                        {
                            if (driverFlavour.Name == item.Name)
                                subPrice += driverFlavour.Price * item.Amount;
                        }
                    }
                    totPrice += subPrice;
                }
                var orderTotalPriceResource = mapper.Map<DriverResource, OrderTotalPriceResource>(resultDriver);
                orderTotalPriceResource.TotalPrice = totPrice;
                orderTotalPriceResource.OrderID = currentOrder.OrderID;
                listPriceResources.Add(orderTotalPriceResource);
            }
            return listPriceResources;
        }

        public async Task<bool> UpdateFlavoursPrice(string email, FlavourFrountend[] flavours)
        {
            var driver = await usersRepo.GetDriverByEmail(email);

            if (flavours == null || driver == null)
            {
                return false;
            }
            List<DriverFlavour> driverFlavours = await usersRepo.GetDriversFlavours(email);
            if (driverFlavours == null || driverFlavours.Count <= 0)
            {
                driverFlavours = new List<DriverFlavour>();
                foreach (var item in flavours)
                {
                    var flavour = orderContext.Flavours.SingleOrDefault(f => f.Name == item.Name);
                    driverFlavours.Add(new DriverFlavour { FlavourID = flavour.FlavourID, DriverID = driver.DriverID, Price = item.Price });
                }
            }
            else
            {
                foreach (var item in flavours)
                {
                    var flavour = orderContext.Flavours.SingleOrDefault(f => f.Name == item.Name);
                    var index = driverFlavours.IndexOf(driverFlavours.SingleOrDefault(df => df.DriverID == driver.DriverID && df.FlavourID == flavour.FlavourID));
                    if (index != -1)
                    {
                        driverFlavours[index].Price = item.Price;
                    }
                }
            }
            driver.DriverFlavours = driverFlavours;
            var result = await userContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDriversLocation(string email, Location location)
        {
            try
            {
                var driver = await usersRepo.GetDriverByEmail(email);
                driver.Location.latitude = location.latitude;
                driver.Location.longitude = location.longitude;
                var result = await userContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
