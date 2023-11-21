using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CateringManagement.Models.DTO
{
    public class UserInfoDTO
    {
        [DataMember(EmitDefaultValue = false)]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 kí tự")]
        public string Email { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 kí tự")]
        public string? password { get; set; }

        public string? Image { get; set; }
    }
}
