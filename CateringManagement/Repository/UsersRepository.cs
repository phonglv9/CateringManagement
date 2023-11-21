using CateringManagement.Common;
using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.Context;
using EntityFramework.DomainClass;
using Microsoft.EntityFrameworkCore;

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
        public async Task<string> GetMaxEmployeeId()
        {
            var sinhViens = await db.Users
                .Where(sv => sv.EmployeeId != null && sv.EmployeeId.StartsWith("Us"))
                .ToListAsync();

            var maxMaSv = sinhViens
                .Select(sv => sv.EmployeeId.Substring(2))
                .Where(substring => int.TryParse(substring, out int parsedValue))
                .Select(parsedValue => parsedValue);

            var result = maxMaSv.Any() ? maxMaSv.Max().ToString() : "0";

            return result;
        }



    }
}
