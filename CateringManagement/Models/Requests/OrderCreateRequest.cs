namespace CateringManagement.Models.Requests
{
    public class OrderCreateRequest
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime PickupDate { get; set; }
        public List<OrderMealCreateRequest> Meals { get; set; }
    }

    public class OrderMealCreateRequest
    {
        public Guid MealId { get; set; }
        public int Quantity { get; set; }
    }
}
