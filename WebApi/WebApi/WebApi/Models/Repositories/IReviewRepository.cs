using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Resources;
using WebApi.Models.Review;

namespace WebApi.Models.Repositories
{
    public interface IReviewRepository
    {
        Task<bool> AddDriverReview(ReviewResource review);
        IEnumerable<ReviewResource> GetDriverReviews(Driver driver);
    }
}