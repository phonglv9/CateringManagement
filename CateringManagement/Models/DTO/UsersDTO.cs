using System.ComponentModel.DataAnnotations.Schema;

namespace CateringManagement.Models.DTO
{
    public class UsersDTO
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        [Column(TypeName = "date")]
        public string DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string? Image { get; set; }
    }
}
