using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    [Table("Addresses")]
    public class Address
    {
        public int AddressID { get; set; }

        [Required(ErrorMessage = "De straatnaam is verplicht")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Straatnaam moet tussen 5 en 50 karakters bevatten")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Het huisnummer is verplicht")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Huisnummer moet tussen 1 en 10 karakters bevatten")]
        public string StreetNumber { get; set; }

        [Required(ErrorMessage = "De postcode is verplicht")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "Postcode moet tussen 4 en 6 karakters bevatten")]
        public string ZipCode { get; set; }
    }
}
