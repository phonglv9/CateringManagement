using Microsoft.AspNetCore.Identity;

namespace DAL.DomainClass
{
    public class AppUsers : IdentityRole<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Dob { get; set; }
    }
}
