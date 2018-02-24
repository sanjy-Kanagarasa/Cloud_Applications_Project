using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    [Table("ContactInformation")]
    public class ContactInformation
    {
        public int ContactInformationID { get; set; }

        [Required(ErrorMessage = "De email adres is verplicht")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is niet geldig")]
        [StringLength(50, ErrorMessage = "Email mag maximaal 50 karakters bevatten")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Het telefoonnummer is verplicht")]
        [StringLength(13, MinimumLength = 9, ErrorMessage = "Telefoonnummer moet tussen 9 en 13 cijfers bevatten")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enkel cijfers zijn toegelaten")]
        public string PhoneNumber { get; set; }

        // Navigation properties
        //public Customer Customer { get; set; }
        public Address Address { get; set; }
    }
}
