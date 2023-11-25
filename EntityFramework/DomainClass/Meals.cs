namespace DAL.DomainClass
{
    public class Meals : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string? Image { get; set; }
        public virtual ICollection<MealIngredients> MealIngredients { get; set; }
    }
}
