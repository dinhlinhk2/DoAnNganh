using System.ComponentModel.DataAnnotations;

namespace Demo.Areas.Admin.Models
{
    public class AdminLoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn phải nhập tối thiểu 5 ký tự")]
        public string Password { get; set; }
    }
}
