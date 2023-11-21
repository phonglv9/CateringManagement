using Microsoft.AspNetCore.Identity;

namespace EntityFramework.DomainClass
{
    public class AppRoles : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
