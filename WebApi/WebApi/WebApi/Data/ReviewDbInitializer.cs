using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public class ReviewDbInitializer
    {
        public static void Initialize(ReviewContext context)
        {
            context.Database.EnsureCreated();

            if (context.DriverReview.Any())
            {
                return;
            }
        }
    }
}
