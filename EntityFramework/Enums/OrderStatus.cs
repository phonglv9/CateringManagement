using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enums
{
    public enum OrderStatus
    {
        [Description("Đang xử lý")]
        InProgress = 0,

        [Description("Hoàn thành")]
        Done = 1
    }
}
