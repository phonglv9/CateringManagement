using CateringManagement.Common;
using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.DomainClass;

namespace CateringManagement.Repository
{
    public class UsersRepository: GenericRepository<Users>
    {

        public async Task< List<UsersDTO>> getLstUsers()
        {
           return await db.Users.Select(user => new UsersDTO
            {
                EmployeeId = user.EmployeeId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreateDate = TextUtils.ConvertDateTimeToString(user.CreateDate),
                UpdateDate = TextUtils.ConvertDateTimeToString(user.UpdateDate),
                DateOfBirth = TextUtils.ConvertDateToString(user.DateOfBirth),
                Sex = user.Sex == 1 ? "Male": "Female",
                Status = user.Status == 1 ? "Active" : "InActive",
                Image = user.Image

            }).ToListAsync();
        }



    }
}
