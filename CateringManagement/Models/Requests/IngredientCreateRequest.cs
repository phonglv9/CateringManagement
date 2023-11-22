using static DAL.Enums.CommonEnum;

namespace CateringManagement.Models.Requests
{
    public class IngredientCreateRequest
    {
        public string Name { get; set; }
        public UnitEnum Unit { get; set; }
        public decimal PriceUnit { get; set; }
    }
}
