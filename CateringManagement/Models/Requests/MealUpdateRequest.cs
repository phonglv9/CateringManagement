namespace CateringManagement.Models.Requests
{
    public class MealUpdateRequest
    {
        public string Name { get; set; }
        public List<MealIgredientCreateRequest> Ingredients { get; set; }
    }

    public class MealIgredientUpdateRequest
    {
        public Guid IngredientId { get; set; }
        public int Quantity { get; set; }
    }
}
