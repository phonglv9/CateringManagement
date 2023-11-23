namespace CateringManagement.Models.DTO
{
    public class SimpleIngredientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
