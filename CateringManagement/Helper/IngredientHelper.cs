using static DAL.Enums.CommonEnum;

namespace CateringManagement.Helper
{
    public static class IngredientHelper
    {
        public static string GetUnitName(UnitEnum unit)
        {
            return unit switch
            {
                UnitEnum.Ml => "Ml",
                UnitEnum.Gram => "Gram",
                _ => "Undefine"
            };
        }
    }
}
