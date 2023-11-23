namespace CateringManagement.Models.DTO
{
    public class MealDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<MealIngredientDTO> Ingredients { get; set; }
    }

    public class MealIngredientDTO
    {
        public Guid IngredientId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
