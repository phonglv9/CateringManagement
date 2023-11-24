using DAL.Enums;

namespace CateringManagement.Helper
{
    public static class OrderHelper
    {
        public static string GetOrderStatus(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.InProgress => "In-progress",
                OrderStatus.Done => "Done",
                _ => "Undefine"
            };
        }
    }
}
