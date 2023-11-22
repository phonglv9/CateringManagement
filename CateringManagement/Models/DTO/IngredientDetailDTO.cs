namespace CateringManagement.Models.DTO
{
    public class IngredientDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
