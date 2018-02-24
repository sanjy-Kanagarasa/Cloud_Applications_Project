using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Orders;
using WebApi.Models.Resources;
using WebApi.Models.Users;

namespace WebApi.Models.Repositories
{
    public interface IUsersRepository
    {
        Task<Customer> GetCustomerByEmail(string email);
        Task<Driver> GetDriverByEmail(string email);
        Task<Customer> GetCustomerById(int id);
        Task<Driver> GetDriverById(int id);
        Task<UserRoleTypes> CustomerOrDriver(string email);
        Task<bool> CanUserLogin(LoginForm loginUser, UserRoleTypes userRole);
        Task<IEnumerable<Customer>> GetCustomers();
        Task<IEnumerable<Driver>> GetDrivers();
        IEnumerable<DriverResource> DriverToDriversResource(IEnumerable<Driver> drivers);
        IEnumerable<CustomerResource> CustomerToCustomersResource(IEnumerable<Customer> customers);
        Task<List<DriverFlavour>> GetDriversFlavours(string email);
        Task<bool> CreateDriverFlavourTable(Driver driver);
        Task<bool> UpdateCustomerLocation(string email, Location location);
    }
}