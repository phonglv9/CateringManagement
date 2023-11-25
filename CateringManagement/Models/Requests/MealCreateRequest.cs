namespace CateringManagement.Models.Requests
{
    public class MealCreateRequest
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public string Description { get; set; }
        public List<MealIgredientCreateRequest> Ingredients { get; set; }
    }

    public class MealIgredientCreateRequest
    {
        public Guid IngredientId { get; set; }
        public int Quantity { get; set; }
    }
}
