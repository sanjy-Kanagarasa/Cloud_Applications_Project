using WebApi.Models.Users;

namespace WebApi.Models.Resources
{
    public class CustomerResource
    {
        public string Email { get; set; }
        public Location Location { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
    }
}
