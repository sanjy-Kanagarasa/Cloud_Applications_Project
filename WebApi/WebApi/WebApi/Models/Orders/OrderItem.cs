using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Orders
{
    [Table("OrderItems")]
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public double TotalPrice { get; set; }


        public ICollection<OrderItemFlavour> OrderItemFlavours { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }

        public OrderItem()
        {
            OrderItemFlavours = new Collection<OrderItemFlavour>();
        }
    }
}
