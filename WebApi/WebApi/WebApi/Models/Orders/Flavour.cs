using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Orders
{
    [Table("Flavours")]
    public class Flavour
    {
        
        public int FlavourID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ICollection<OrderItemFlavour> OrderItemFlavours { get; set; }
        public ICollection<DriverFlavour> DriverFlavours { get; set; }

        public Flavour()
        {
            OrderItemFlavours = new Collection<OrderItemFlavour>();
            DriverFlavours = new Collection<DriverFlavour>();
        }
    }
}
