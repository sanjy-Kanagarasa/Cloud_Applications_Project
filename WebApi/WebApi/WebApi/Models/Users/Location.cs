using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Users
{
    public class Location
    {
        public int LocationID { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
    }
}
