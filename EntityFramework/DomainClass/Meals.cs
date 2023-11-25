namespace DAL.DomainClass
{
    public class Meals : BaseEntity
    {
        public string Name { get; set; }
        public Guid MealCategoryId { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MealIngredients> MealIngredients { get; set; }
        public MealCategories MealCategory { get; set; }
    }
}
