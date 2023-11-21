using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enums
{
    public class CommonEnum
    {
        public enum UnitEnum
        {
            [Description("Milliter")]
            Ml = 0,

            [Description("Gram")]
            Gram = 1,
        }
    }
}
