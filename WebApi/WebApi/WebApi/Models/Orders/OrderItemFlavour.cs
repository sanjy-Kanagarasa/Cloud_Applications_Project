using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Orders
{
    [Table("OrderItemFlavours")]
    public class OrderItemFlavour
    {
        //public int OrderItemFlavourID { get; set; }
        public int OrderItemID { get; set; }
        public int FlavourID { get; set; }

        [DefaultValue(1)]
        public int Amount { get; set; }

        public Flavour Flavour { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
