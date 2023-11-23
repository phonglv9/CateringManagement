using CateringManagement.Common;
using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CateringManagement.Repository
{
    public class UsersRepository : GenericRepository<Users>
    {

        public async Task<List<UsersDTO>> getLstUsers(int? role, string? searching)
        {
            try
            {
                var query = db.Users.Where(u => u.IsDeleted != 1);

                if (role.HasValue && role != -1)
                {
                    query = query.Where(u => ((int)u.Role) == role);
                }

                if (!string.IsNullOrEmpty(searching))
                {

                    query = query.Where(u =>
                        u.FirstName.Contains(searching) ||
                        u.LastName.Contains(searching) ||
                        u.Email.Contains(searching)
                    );
                }

                var lstUsers = await query.Select(user => new UsersDTO
                {
                    EmployeeId = user.EmployeeId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreateDate =  user.CreatedAt.ToString(),
                    UpdateDate = user.UpdatedAt.ToString(),
                    DateOfBirth = TextUtils.ConvertDateToString(user.DateOfBirth),
                    Sex = user.Sex == 1 ? "Male" : "Female",
                    Status = user.Status == 1 ? "Active" : "InActive",
                    Image = user.Image,
                    Role = user.Role.ToString(),

                }).OrderByDescending(c => c.CreateDate).ToListAsync();


                return lstUsers;
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

        public async Task<int> DeleteUserByEmployeeId(string employeeId)
        {
            var user = await db.Users.Where(c => c.EmployeeId == employeeId).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsDeleted = 1;
                return await Update(user);
            }

            return 0;
        }
        public async Task<Users> GetUserEmployeeId(string employeeId)
        {
            try
            {
                var user = await db.Users.Where(c => c.EmployeeId == employeeId).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Users GetUserByEmail(string email)
        {
            try
            {
                var user =  db.Users.Where(c => c.Email == email).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Users> GetUserEmail(string email)
        {
            try
            {
                var user = await db.Users.Where(c => c.Email == email).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
