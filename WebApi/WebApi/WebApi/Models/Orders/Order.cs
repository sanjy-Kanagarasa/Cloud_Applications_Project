using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Users;

namespace WebApi.Models.Orders
{
    [Table("Orders")]
    public class Order
    {
        public int OrderID { get; set; }
        public double TotalPrice { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Location Location { get; set; }
        public OrderStatus Status { get; set; }
        public bool Payed { get; set; }


        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int DriverID { get; set; }
        public Driver Driver { get; set; }
    }
}
