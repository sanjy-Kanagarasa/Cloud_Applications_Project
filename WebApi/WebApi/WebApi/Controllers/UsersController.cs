using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Repositories;
using WebApi.Models.Resources;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UserContext context;
        private readonly IMapper mapper;
        private readonly IUsersRepository usersRepo;
        private readonly IOrderRepository orderRepository;

        public UsersController(UserContext _context, IMapper _mapper, IUsersRepository usersRepo, IOrderRepository orderRepository) {
            context = _context;
            mapper = _mapper;
            this.usersRepo = usersRepo;
            this.orderRepository = orderRepository;
        }

        //////////////////////////////////// 
        ///     GET: api/drivers    ////////
        ///     GET: api/customers  ////////
        //////////////////////////////////// 
        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await usersRepo.GetCustomers();
            if (customers != null)
            {
                return Ok(usersRepo.CustomerToCustomersResource(customers));
            }
            return NotFound("No customers found");
        }

        [HttpGet("drivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await usersRepo.GetDrivers();
            if (drivers != null)
            {
                return Ok(usersRepo.DriverToDriversResource(drivers));
            }
            return NotFound("No customers found");
        }


        ///////////////////////////////////////////////
        ///     GET: api/users/chingiz@uber.be    /////
        ///////////////////////////////////////////////
        [HttpGet("{email}"), ActionName("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var customerOrDriver = await usersRepo.CustomerOrDriver(email);

            if (customerOrDriver == UserRoleTypes.NOTFOUND)
                return NotFound("Email " + email + " not found");

            if (customerOrDriver == UserRoleTypes.CUSTOMER)
                return Ok( mapper.Map<Customer, CustomerResource>(await usersRepo.GetCustomerByEmail(email)));

            else if(customerOrDriver == UserRoleTypes.DRIVER)
                return Ok(mapper.Map<Driver, DriverResource>(await usersRepo.GetDriverByEmail(email))); 

            return BadRequest();
            
        }


        ///////////////////////////////// 
        ///     POST: api/Users     /////
        /// /////////////////////////////
        [HttpPost, ActionName("CreateNewUser")]
        public async Task<IActionResult> CreateNewUserAsync([FromBody] RegistrationForm newUser)
        {

            // placeholder for feedback
            var newUserSavedToDatabase = false;

            // check if form was filled in correctly
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customerOrDriver = await usersRepo.CustomerOrDriver(newUser.Email);
            if (customerOrDriver == UserRoleTypes.CUSTOMER || customerOrDriver == UserRoleTypes.DRIVER)
                return BadRequest("Email must be unique");

            // Check users role and create customer or driver object accordingly
            // Create new customer object
            if (newUser.UserRoleType == (int)UserRoleTypes.CUSTOMER)
            {
                Customer user = new Customer
                {
                    RegistrationDate = DateTime.Now,
                    Location = new Location { latitude = (float)(51.2001783), longitude = (float)4.4327702 },
                    ContactInformation = new ContactInformation { Address = new Address() }
                };
                var customer = mapper.Map<RegistrationForm, Customer>(newUser, user);

                try
                {
                    await context.Customers.AddAsync(customer);
                    await context.SaveChangesAsync();
                    newUserSavedToDatabase = true;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }


            }
             
            // Create new driver object
            else if (newUser.UserRoleType == (int)UserRoleTypes.DRIVER)
            {                
                Driver user = new Driver
                {
                    RegistrationDate = DateTime.Now,
                    Location = new Location { latitude = (float)(51.211170), longitude = (float)(4.410528) },
                    ContactInformation = new ContactInformation { Address = new Address() }
                };
                var driver = mapper.Map<RegistrationForm, Driver>(newUser, user);
                var flavours = await orderRepository.GetFlavoursAsync();
                

                try
                {
                    await context.Drivers.AddAsync(driver);
                    await context.SaveChangesAsync();
                    newUserSavedToDatabase = true;

                    foreach (var flavour in flavours)
                    {
                        await context.DriverFlavours.AddAsync(new DriverFlavour
                        {
                            DriverID = driver.DriverID,
                            FlavourID = flavours.Single(f => f.Name == flavour.Name).FlavourID,
                            Price = flavour.Price
                        });
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }

            return Ok(newUserSavedToDatabase);
        }

        // POST: api/Users/sanjy-driver@uber.be
        [HttpPost("{email}")]
        public async Task<IActionResult> UserLogin(string email, [FromBody]LoginForm loginUser)
        {
            var customerOrDriver = await usersRepo.CustomerOrDriver(loginUser.Email);
            if (customerOrDriver == UserRoleTypes.NOTFOUND)
                return Ok("You have to register first");
            bool canAccess = false;
            if (customerOrDriver != UserRoleTypes.NOTFOUND)
            {
                canAccess = await usersRepo.CanUserLogin(loginUser, customerOrDriver);
                if (canAccess)
                    return Ok(customerOrDriver.ToString());
                else
                    return Ok(canAccess);

            }
            return Ok(canAccess);
        }

        // PUT: api/Users/sanjy-driver@uber.be
        [HttpPut("{email}")]
        public async Task<IActionResult> EditUserByEmail(string email, [FromBody]RegistrationForm newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (newUser.UserRoleType == (int)UserRoleTypes.CUSTOMER)
            {
                var user = await usersRepo.GetCustomerByEmail(email);
                if (user == null)
                    return NotFound(false);
                var customer = mapper.Map<RegistrationForm, Customer>(newUser, user);
                context.Customers.Update(customer);
                await context.SaveChangesAsync();
                return Ok(true);

            }

            // Update driver object
            else if (newUser.UserRoleType == (int)UserRoleTypes.DRIVER)
            {
                var user = await usersRepo.GetDriverByEmail(email);
                if (user == null)
                    return NotFound(false);
                var driver = mapper.Map<RegistrationForm, Driver>(newUser, (Driver)user);
                context.Drivers.Update(driver);
                await context.SaveChangesAsync();
                return Ok(true);
            }
            return BadRequest(false);
        }
    
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            var customer = await usersRepo.GetCustomerByEmail(email);
            if (customer != null)
            {
                //context.Remove(user);
                context.RemoveRange(customer.ContactInformation.Address);
                context.RemoveRange(customer.ContactInformation);
                context.RemoveRange(customer);
                context.SaveChanges();
                return Ok(true);
            }
            var driver = await usersRepo.GetDriverByEmail(email);
            if (driver != null)
            {
                //context.Remove(user);
                context.RemoveRange(driver.ContactInformation.Address);
                context.RemoveRange(driver.ContactInformation);
                context.RemoveRange(driver);
                context.SaveChanges();
                return Ok(true);
            }
            return NotFound(false);
        }

        [HttpPost("location/{email}"), ActionName("UpdateCustomerLocation")]
        public async Task<IActionResult> UpdateCustomerLocation(string email, [FromBody]Location location)
        {
            var result = await usersRepo.UpdateCustomerLocation(email, location);
            return Ok(result);
        }
    }
}
