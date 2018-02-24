using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Orders
{
    [Table("DriverFlavours")]
    public class DriverFlavour
    {
        public int DriverID { get; set; }
        public int FlavourID { get; set; }
        public double Price { get; set; }

        public Flavour Flavour { get; set; }
        public Driver Driver { get; set; }
    }
}
