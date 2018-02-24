using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Resources;

namespace WebApi.Models.Resources
{
    public class OrderResource
    {
        public ShoppingCart ShoppingCart { get; set; }

        public DriverResource Driver { get; set; }
        public CustomerResource Customer { get; set; }

        public double TotalPrice { get; set; }
        public int OrderID { get; set; }
    }
}
