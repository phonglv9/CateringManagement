namespace CateringManagement.Models.DTO
{
    public class MealCategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CanDelete { get; set; }
    }
}
