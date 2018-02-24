using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Users;

namespace WebApi.Models.Orders.Resources
{
    public class OrderTotalPriceResource
    {
        public string Email { get; set; }
        public Location Location { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OrderID { get; set; }
        public double TotalPrice { get; set; }

    }
}
