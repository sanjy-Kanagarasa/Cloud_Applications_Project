using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Resources
{
    public class ReviewResource
    {

        [Required(ErrorMessage = "Score is verplicht")]
        [Range(0, 5, ErrorMessage = "Can only be between 0 .. 5")]
        public int Rating { get; set; }
        public string Review { get; set; }
        public string ReviewToEmail { get; set; }
        public string ReviewFromEmail { get; set; }
        public string ReviewerName { get; set; }
    }
}
