using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.Context;
using EntityFramework.DomainClass;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Repository
{
    public class UsersRepository: GenericRepository<Users>
    {

         
        public  async List<UsersDTO> getLstUsers()
        {

           return  await db.Users.Select(user => new UsersDTO
            {
                EmployeeId = user.EmployeeId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreateDate = user.CreateDate,
                UpdateDate = user.UpdateDate.ToString(),
                DateOfBirth = user.DateOfBirth,
                Sex = user.Sex == 1 ? "Male": "Female",
                Status = user.Status == 1 ? "Active" : "InActive",
                Image = user.Image

            }).ToListAsync();
        }



    }
}
