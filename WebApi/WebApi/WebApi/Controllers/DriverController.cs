using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Resources;
using WebApi.Models.Repositories;
using AutoMapper;
using WebApi.Models;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/driver")]
    public class DriverController : Controller
    {
        private readonly IUsersRepository usersRepo;
        private readonly IDriverRepository driverRepo;
        private readonly IMapper mapper;

        public DriverController( IUsersRepository usersRepo, IDriverRepository driverRepo, IMapper mapper)
        {
            this.usersRepo = usersRepo;
            this.driverRepo = driverRepo;
            this.mapper = mapper;
        }

        [HttpPost("{email}"), ActionName("UpdateFlavoursPrice")]
        public async Task<IActionResult> UpdateFlavoursPrice(string email, [FromBody]FlavourFrountend[] flavours)
        {
            var result = await driverRepo.UpdateFlavoursPrice(email, flavours);
            return Ok(result);
        }


        [HttpPost("location/{email}"), ActionName("UpdateDriversLocation")]
        public async Task<IActionResult> UpdateDriversLocation(string email, [FromBody]Location location)
        {
            var result = await driverRepo.UpdateDriversLocation(email, location);
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetFlavoursPrice(string email)
        {
            var driver = await driverRepo.GetFlavoursPrice(email);
            var result  = mapper.Map<Driver, DriverResource>(driver);

            if (result !=null)
                return Ok(result);
            return NotFound();
        }
    }
}