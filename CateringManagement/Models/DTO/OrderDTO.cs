using DAL.Enums;

namespace CateringManagement.Models.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public OrderStatus Status { get; set; }
        public string CreatedTime { get; set; }
        public string PickupDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
