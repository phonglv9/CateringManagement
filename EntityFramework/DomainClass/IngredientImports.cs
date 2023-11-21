namespace DAL.DomainClass
{
    public class IngredientImports : BaseEntity // Bảng Nhập Nguyên Liệu
    {
        public Guid IngredientId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiredDate { get; set; }


        public Ingredients Ingredient { get; set; }
    }
}
