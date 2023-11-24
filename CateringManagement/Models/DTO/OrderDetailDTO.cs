namespace CateringManagement.Models.DTO
{
    public class OrderDetailDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Status { get; set; }
        public string CreatedTime { get; set; }
        public string PickupDate { get; set; }
        public DateTime PickupDateDT { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal SellPrice { get; set; }
        public List<OrderMealDTO> Meals { get; set; }
    }

    public class OrderMealDTO
    {
        public Guid MealId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
