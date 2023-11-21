namespace DAL.DomainClass
{
    public class OrderDetail : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid MealId { get; set; }
        public int Quantity { get; set; }

        public Orders Order { get; set; }
        public Meals Meal { get; set; }

    }
}
