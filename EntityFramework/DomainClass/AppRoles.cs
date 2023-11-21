using Microsoft.AspNetCore.Identity;

namespace DAL.DomainClass
{
    public class AppRoles : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
