using System.ComponentModel;

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
