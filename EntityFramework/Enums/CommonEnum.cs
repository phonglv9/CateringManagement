using System.ComponentModel;

namespace DAL.Enums
{
    public class CommonEnum
    {
        public enum UnitEnum
        {
            [Description("Milliter")]
            Ml = 1,

            [Description("Gram")]
            Gram = 2,
        }

        public enum StatusEnum
        {
            [Description("Unavailable")]
            Unavailable = 0,

            [Description("Available")]
            Available = 1,
        }
    }
}
