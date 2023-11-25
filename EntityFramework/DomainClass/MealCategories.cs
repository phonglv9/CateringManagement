namespace DAL.DomainClass
{
    public class MealCategories : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Meals> Meals { get; set; }
    }
}
