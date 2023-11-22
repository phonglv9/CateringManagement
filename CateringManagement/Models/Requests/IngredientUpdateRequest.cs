using static DAL.Enums.CommonEnum;

namespace CateringManagement.Models.Requests
{
    public class IngredientUpdateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
