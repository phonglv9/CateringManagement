using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DomainClass
{
    public class Orders
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime PickupTime { get; set; } // thời gian dự kiến lấy hàng
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
