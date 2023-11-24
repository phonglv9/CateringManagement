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
                    Status = OrderHelper.GetOrderStatus(x.Status),
                    TotalPrice = x.TotalPrice,
                    SellPrice = x.SellPrice
                })
                .ToListAsync();

            return data;
        }

        public async Task<int> InsertOrderMealList(List<OrderDetail> orderMeals)
        {
            db.OrderDetails.AddRange(orderMeals);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderMealList(List<OrderDetail> orderMeals, Guid userId)
        {
            List<OrderDetail> dataUpdate = new();
            foreach (var item in orderMeals)
            {
                item.IsDeleted = 1;
                item.UpdatedBy = userId;
                dataUpdate.Add(item);
            }
            db.OrderDetails.UpdateRange(dataUpdate);
            return await db.SaveChangesAsync();
        }
    }
}
