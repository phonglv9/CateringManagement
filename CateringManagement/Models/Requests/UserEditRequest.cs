using DAL.Enums;

namespace CateringManagement.Models.Requests
{
    public class UserEditRequest
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Status { get; set; }
        public int Sex { get; set; }
        public UserPosition Role { get; set; }
    }
}
