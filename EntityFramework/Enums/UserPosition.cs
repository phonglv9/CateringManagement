using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enums
{
    public enum UserPosition
    {
        [Description("Quản lý")]
        Admin = 0,

        [Description("Quản lý kho")]
        Storage = 1,

        [Description("Đầu bếp")]
        Chef = 2,

        [Description("Lễ tân")]
        Reception = 3
    }
}
