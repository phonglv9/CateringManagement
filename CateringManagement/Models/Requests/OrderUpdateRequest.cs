namespace CateringManagement.Models.Requests
{
    public class OrderUpdateRequest
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime PickupDate { get; set; }
        public List<OrderMealUpdateRequest> Meals { get; set; }
    }

    public class OrderMealUpdateRequest
    {
        public Guid MealId { get; set; }
        public int Quantity { get; set; }
    }
}
