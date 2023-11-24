using DAL.DomainClass;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class Users_Configuration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");
            builder.HasData(
                new Users
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = "Ad1",
                    FirstName = "admin",
                    LastName = "",
                    Email = "admin@gmail.com",
                    Password = "admin123",
                    DateOfBirth = DateTime.UtcNow,
                    Sex = 1,
                    Status = 1,
                    Image = "",
                    Role = UserPosition.Admin,
                    IsDeleted = 0,
                    CreatedBy = null,
                    UpdatedBy = null,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
        }
    }
}
