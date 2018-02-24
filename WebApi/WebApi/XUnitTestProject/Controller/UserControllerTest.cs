using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Repositories;
using Xunit;
using System.Linq;
using WebApi.Models.Users;

namespace XUnitTestProject.Controller
{
    public class UserControllerTest 
    {
        private readonly Mock<IUsersRepository> _mockUserRepo;
        private readonly Mock<IDriverRepository> _mockDriverRepo;
        private readonly Mock<IOrderRepository> _mockOrderRepo;
        private readonly IMapper mapper;

        private readonly UsersController _userController;
        private readonly UserContext _context;

        public UserControllerTest()        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase()
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            _context = new UserContext(options);
            _context.Database.EnsureCreated();

            UserDbInitializer userDbInit = new UserDbInitializer();
            
            _mockUserRepo = new Mock<IUsersRepository>();
            _mockDriverRepo = new Mock<IDriverRepository>();
            _mockOrderRepo = new Mock<IOrderRepository>();
            _userController = new UsersController(_context,  mapper, _mockUserRepo.Object, _mockOrderRepo.Object);

        }

        [Fact]
        public async Task CustomerTester()
        {
            var customers = GetCustomerList();
            _mockUserRepo.Setup(e => e.GetCustomers()).Returns(Task.FromResult(customers));

            var result = await _userController.GetAllCustomers() as ActionResult;
            var total = await _mockUserRepo.Object.GetCustomers();

            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(3, total.Count());
        }

        [Fact]
        public async Task DriverTester()
        {
            var drivers = GetDriversList();
            _mockUserRepo.Setup(e => e.GetDrivers()).Returns(Task.FromResult(drivers));

            var result = await _userController.GetAllDrivers() as ActionResult;
            var total = await _mockUserRepo.Object.GetDrivers();

            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(4, total.Count());
        }

        [Fact]
        public async Task CustomerOrDriverTester()
        {
            var drivers = GetDriversList();
            _mockUserRepo.Setup(e => e.GetDrivers()).Returns(Task.FromResult(drivers));

            var result = await _userController.GetAllDrivers() as ActionResult;
            var data = await _mockUserRepo.Object.CustomerOrDriver("sanjy-customer@uber.be");

            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(UserRoleTypes.CUSTOMER, data);
        }

        [Fact]
        public async Task GetCustomerByEmailTest()
        {
            var customers = GetCustomerList();
            _mockUserRepo.Setup(e => e.GetCustomers()).Returns(Task.FromResult(customers));
            
            var result = await _userController.GetUserByEmail("chingiz-customer@uber.be") as ActionResult;
            var data = await _mockUserRepo.Object.GetCustomerByEmail("chingiz-customer@uber.be");

            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsType<Customer>(data);
        }
        [Fact]
        public async Task GetDriverByEmailTest()
        {
            var drivers = GetDriversList();
            _mockUserRepo.Setup(e => e.GetDrivers()).Returns(Task.FromResult(drivers));

            var result = await _userController.GetUserByEmail("chingiz-driver@uber.be") as ActionResult;
            var data = await _mockUserRepo.Object.GetCustomerByEmail("chingiz-driver@uber.be");

            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            Assert.IsType<Customer>(data);
        }

        private IEnumerable<Customer> GetCustomerList()
        {
            IEnumerable<Customer> customers = new List<Customer>
            {
                new Customer{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now, Location = new Location {latitude = (float) (51.2304895), longitude = (float) (4.4774069)}, ContactInformation = new ContactInformation { Email = "sanjy-customer@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Customer{ FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now, Location = new Location {latitude = (float) (51.2001783), longitude = (float) 4.4327702}, ContactInformation = new ContactInformation { Email = "chingiz-customer@uber.be", PhoneNumber = "4443332221111", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Customer{ FirstName = "Stijn", LastName = "Pittomvils", Password = "test123", RegistrationDate = DateTime.Now, Location = new Location {latitude = (float) (51.1892618), longitude = (float) 4.7385712}, ContactInformation = new ContactInformation { Email = "stijn-customer@uber.be", PhoneNumber = "99999999999", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},

            };
            return customers;
        }
        private IEnumerable<Driver> GetDriversList()
        {
            IEnumerable<Driver> drivers = new List<Driver>
            {
                new Driver{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.2301299), longitude = (float) ( 4.4161723) }, ContactInformation = new ContactInformation { Email = "sanjy-driver@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.2469382), longitude =  (float) (4.45203781)}, ContactInformation = new ContactInformation { Email = "chingiz-driver@uber.be", PhoneNumber = "4443332221111", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Stijn", LastName = "Pittomvils", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.25596345), longitude =  (float) (4.42354202)}, ContactInformation = new ContactInformation { Email = "stijn-driver@uber.be", PhoneNumber = "99999999999", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}, IsApproved = true },
                new Driver{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now,  Location = new Location {latitude = (float) (51.22931236), longitude =  (float) (4.50971604)}, ContactInformation = new ContactInformation { Email = "sanjy-driver1@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
            };
            return drivers;
        }


    }
}
