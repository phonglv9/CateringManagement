namespace CateringManagement.Models.DTO;

public class IngredientDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ImportDate { get; set; }
    public string ExpiredDate { get; set; }
    public string Status { get; set; }
}
