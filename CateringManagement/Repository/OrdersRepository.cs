using CateringManagement.Common;
using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Repository
{
    public class OrdersRepository : GenericRepository<Orders>
    {
        public async Task<List<OrderDTO>> GetAllData()
        {
            var data = await db.Orders.Where(x => x.IsDeleted == 0)
                .Select(x => new OrderDTO
                {
                    Id = x.Id,
                    Code = x.OrderCode,
                    CustomerName = x.CustomerName,
                    CustomerPhone = x.CustomerPhone,
                    CreatedTime = TextUtils.ConvertDateTimeToString(x.CreatedAt),
                    PickupDate = TextUtils.ConvertDateToString(x.PickupTime),
                    Status = x.Status
                })
                .ToListAsync();

            return data;
        }

        //public async Task<int> InsertOrderIngredientList(List<OrderIngredients> OrderIngredients)
        //{
        //    db.OrderIngredients.AddRange(OrderIngredients);
        //    return await db.SaveChangesAsync();
        //}

        //public async Task<int> DeleteOrderIngredientList(List<OrderIngredients> OrderIngredients)
        //{
        //    List<OrderIngredients> dataUpdate = new();
        //    foreach (var item in OrderIngredients)
        //    {
        //        item.IsDeleted = 1;
        //        dataUpdate.Add(item);
        //    }
        //    db.OrderIngredients.UpdateRange(dataUpdate);
        //    return await db.SaveChangesAsync();
        //}
    }
}
