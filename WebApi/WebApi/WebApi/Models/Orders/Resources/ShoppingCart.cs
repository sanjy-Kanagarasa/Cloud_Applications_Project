using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Users;

namespace WebApi.Models.Orders.Resources
{
    public class ShoppingCart
    {
        public Icecream[] Cart { get; set; }
        public Location Location { get; set; }
    }
}
