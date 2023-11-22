using CateringManagement.Common;
using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Repository
{
    public class UsersRepository : GenericRepository<Users>
    {

        public async Task<List<UsersDTO>> getLstUsers()
        {
            try
            {
                return await db.Users.Where(C => C.IsDeleted != 0).Select(user => new UsersDTO
                {
                    EmployeeId = user.EmployeeId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreateDate =  null, //TextUtils.ConvertDateTimeToString(user.CreatedAt),
                    UpdateDate = null, //TextUtils.ConvertDateTimeToString(user.UpdatedAt),
                    DateOfBirth = TextUtils.ConvertDateToString(user.DateOfBirth),
                    Sex = user.Sex == 1 ? "Male" : "Female",
                    Status = user.Status == 1 ? "Active" : "InActive",
                    Image = user.Image

                }).OrderByDescending(c => c.CreateDate).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
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
