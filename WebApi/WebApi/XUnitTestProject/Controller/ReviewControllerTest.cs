using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Models.Repositories;
using WebApi.Models.Resources;
using Xunit;

namespace XUnitTestProject.Controller
{
    public class ReviewControllerTest
    {
        private readonly Mock<IUsersRepository> _mockUserRepo;
        private readonly Mock<IDriverRepository> _mockDriverRepo;
        private readonly Mock<IReviewRepository> _mockReviewRepo;
        private readonly IMapper mapper;


        private readonly ReviewController _reviewController;

        public ReviewControllerTest()
        {
            _mockUserRepo = new Mock<IUsersRepository>();
            _mockDriverRepo = new Mock<IDriverRepository>();
            _mockReviewRepo = new Mock<IReviewRepository>();
            _reviewController = new ReviewController(_mockUserRepo.Object, _mockDriverRepo.Object, _mockReviewRepo.Object, mapper);
        }
        

        [Fact]
        public async Task ReturnReviewDriverAsync()
        {
            string testEmail = "sanjy-driver1@uber.be";
         
            var result = await _reviewController.GetReviewsOfDriver(testEmail) as ActionResult;

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task postDriverReview()
        {
            var review = new ReviewResource
            {
                Review = "good",
                Rating = 8,
                ReviewFromEmail = "sanjy-customer@uber.be",
                ReviewToEmail = "sanjy-driver@uber.be"
            };
            IActionResult result = await _reviewController.PostDriverReview(review);

            Assert.IsType<BadRequestResult>(result);
        }
        private Driver testDriver = new Driver
        {
            FirstName = "Sanjy",
            LastName = "Kanagarasa"
        };

    }
}
