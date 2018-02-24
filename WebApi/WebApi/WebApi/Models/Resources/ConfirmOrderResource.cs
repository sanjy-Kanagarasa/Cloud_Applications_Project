using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Resources
{
    public class ConfirmOrderResource
    {
        public int OrderID { get; set; }
        public string DriverEmail { get; set; }
        public string CustomerEmail { get; set; }
        public double TotalPrice { get; set; }
    }
}
