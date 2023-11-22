using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;
using static DAL.Enums.CommonEnum;

namespace CateringManagement.Repository
{
    public class IngredientsRepository : GenericRepository<Ingredients>
    {
        public async Task<List<IngredientDTO>> GetAllData()
        {
            var data = await db.Ingredients.Include(x => x.IngredientImports).Where(x => x.IsDeleted == 0)
                .Select(x => new IngredientDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Unit = IngredientHelper.GetUnitName(x.Unit),
                    Quantity = x.Quantity,
                    Price = x.Price,
                    UnitPrice = x.PriceUnit,
                    Status = x.Quantity > 0 ? StatusEnum.Available : StatusEnum.Unavailable
                })
                .ToListAsync();

            return data;
        }

    }
}
