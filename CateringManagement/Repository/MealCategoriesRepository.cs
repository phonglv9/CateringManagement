using CateringManagement.Repository.Genneric;
using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Repository
{
    public class MealCategoriesRepository : GenericRepository<MealCategories>
    {
        public async Task<List<MealCategories>> GetAllData()
        {
            var data = await db.MealCategories.Include(x => x.Meals.Where(y => y.IsDeleted == 0))
                .Where(x => x.IsDeleted == 0)
                .Select(x => new MealCategories
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync();

            return data;
        }
    }
}
