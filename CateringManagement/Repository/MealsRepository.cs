using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Repository
{
    public class MealsRepository : GenericRepository<Meals>
    {
        public async Task<List<MealDTO>> GetAllData()
        {
            var data = await db.Meals.Where(x => x.IsDeleted == 0)
                .Select(x => new MealDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                })
                .ToListAsync();

            return data;
        }
    }
}
