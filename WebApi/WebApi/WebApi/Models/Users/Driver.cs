using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.HelperClasses;
using WebApi.Models.Orders;
using WebApi.Models.Users;

namespace WebApi.Models
{
    [Table("Drivers")]
    public class Driver 
    {
        public int DriverID { get; set; }

        [Required(ErrorMessage = "De voornaam is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Voornaam moet tussen 2 en 50 karakters bevatten")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "De achternaam is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Achternaam moet tussen 2 en 50 karakters bevatten")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [StringLength(64)]
        public string Salt { get; set; }

        [Required(ErrorMessage = "Het wachtwoord is verplicht")]
        [StringLength(256, ErrorMessage = "Het wachtwoord moet tussen {0} en {2} karakters bevatten", MinimumLength = 6)]
        [DataType(DataType.Password)]
        // Hash the passwords
        private string _password;
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                Salt = HashedPasswordWithSalt.getSalt();
                this._password = HashedPasswordWithSalt.getHash(value, Salt);
            }
        }

        public int Rating { get; set; }

        // Navigation Properties

        public ContactInformation ContactInformation { get; set; }

        public Location Location { get; set; }

        [DefaultValue(false)]
        public bool IsApproved { get; set; }

        public ICollection<DriverFlavour> DriverFlavours { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Driver()
        {
            DriverFlavours = new Collection<DriverFlavour>();
            Orders = new Collection<Order>();
        }
    }
    
}
