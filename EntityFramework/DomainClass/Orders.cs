using DAL.Enums;

namespace DAL.DomainClass
{
    public class Orders : BaseEntity
    {
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime PickupTime { get; set; } // thời gian dự kiến lấy hàng
        public decimal TotalPrice { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
