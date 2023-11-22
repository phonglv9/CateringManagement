namespace CateringManagement.Models.DTO
{
    public class ResponseModel
    {
        public int Status { get; set; }
        public string Mess { get; set; }
    }

    public class ResponseModel<T>
    {
        public int Status { get; set; }
        public string Mess { get; set; }
        public T Data { get; set; }
    }
}
