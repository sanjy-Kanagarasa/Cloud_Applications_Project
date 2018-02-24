using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models.Repositories;
using WebApi.Models.Resources;
using WebApi.Models.Review;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Review")]
    public class ReviewController : Controller
    {
        private readonly IUsersRepository userReop;
        private readonly IDriverRepository driverRepo;
        private readonly IReviewRepository reviewRepo;
        private readonly IMapper mapper;

        public ReviewController(IUsersRepository userReop,
                                IDriverRepository driverRepo,
                                IReviewRepository reviewRepo,
                                IMapper mapper)
        {
            this.userReop = userReop;
            this.driverRepo = driverRepo;
            this.reviewRepo = reviewRepo;
            this.mapper = mapper;
        }

        // POST: api/Review
        [HttpPost("driver")]
        public async Task<IActionResult> PostDriverReview([FromBody]ReviewResource review)
        {
            if (ModelState.IsValid)
            {
                var result = await reviewRepo.AddDriverReview(review);
                if(result)
                    return Ok(result);
                else
                    return NotFound("Driver or Customer not found"); 
            }
            return BadRequest(false);
        }

        
        [HttpGet("{email}")]
        public async Task<IActionResult> GetReviewsOfDriver(string email)
        {
            var driver = await userReop.GetDriverByEmail(email);
            if(driver != null)
            {
                return Ok(reviewRepo.GetDriverReviews(driver));
            }

            return BadRequest();
        }


    }
}
