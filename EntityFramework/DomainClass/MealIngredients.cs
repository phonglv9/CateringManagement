namespace DAL.DomainClass
{
    public class MealIngredients : BaseEntity
    {
        public Guid MealId { get; set; }
        public Guid IngredientId { get; set; }
        public int Quantity { get; set; }

        public Meals Meal { get; set; }
        public Ingredients Ingredient { get; set; }

    }
}
