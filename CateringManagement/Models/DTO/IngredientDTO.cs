using static DAL.Enums.CommonEnum;

namespace CateringManagement.Models.DTO;

public class IngredientDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal UnitPrice { get; set; }
    public StatusEnum Status { get; set; }
}
