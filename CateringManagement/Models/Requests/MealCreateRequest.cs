namespace CateringManagement.Models.Requests
{
    public class MealCreateRequest
    {
        public string Name { get; set; }
    }

    public class MealIgredientCreateRequest
    {
        public Guid IgredientId { get; set; }
        public int Quantity { get; set; }
    }
}
