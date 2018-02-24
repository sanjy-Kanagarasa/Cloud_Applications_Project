using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.Resources;
using WebApi.Models.Review;

namespace WebApi.Models.Repositories
{
    public class ReviewRepository: IReviewRepository
    {
        private readonly IUsersRepository userRepo;
        private readonly OrderContext reviewContext;
        private readonly IMapper mapper;
        private readonly UserContext userContext;

        public ReviewRepository(IUsersRepository userRepo,
             OrderContext reviewContext, IMapper mapper, UserContext userContext)
        {
            this.userRepo = userRepo;
            this.reviewContext = reviewContext;
            this.mapper = mapper;
            this.userContext = userContext;
        }

        public async Task<bool> AddDriverReview(ReviewResource review)
        {
            var driver = await userRepo.GetDriverByEmail(review.ReviewToEmail);
            var customer = await userRepo.GetCustomerByEmail(review.ReviewFromEmail);
            if (driver == null || customer == null)
                return false;
            var driverReview = mapper.Map<ReviewResource, DriverReview>(review);
            driverReview.DriverID = driver.DriverID;
            driverReview.CustomerID = customer.CustomerID;
            await reviewContext.DriverReview.AddAsync(driverReview);
            await reviewContext.SaveChangesAsync();
            await SetRatingAvarage(driver);
            return true;
        }

        private async Task<bool> SetRatingAvarage(Driver driver)
        {
            var listReviews = GetDriverReviews(driver);
            int rating = 0;
            foreach (var review in listReviews)
            {
                rating += review.Rating;
            }
            Driver dr = await userContext.Drivers.SingleOrDefaultAsync(d => d.DriverID == driver.DriverID);
            double _rating = rating / (listReviews.Count());
            // if(_rating % listReviews.Count() == 0)
            //     dr.Rating = (int) _rating;
            // else
            //     dr.Rating = Convert.ToInt32(rating / (listReviews.Count()) + 0.5);
            dr.Rating = (int) Math.Ceiling(_rating);
            await userContext.SaveChangesAsync();
            return true;

        }

        public IEnumerable<ReviewResource> GetDriverReviews(Driver driver)
        {
            var result = reviewContext.DriverReview
                .Where(dr => dr.DriverID == driver.DriverID)
                .Include(dr => dr.Customer)
                    .ThenInclude(c => c.ContactInformation)
                .ToList();
            return mapper.Map<List<DriverReview>, List<ReviewResource>>(result);
        }
    }
}
