namespace CateringManagement.Models.Requests
{
    public class ImportIngredientToStorageRequest
    {
        public Guid IngredientId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
