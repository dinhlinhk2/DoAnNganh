using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Demo.ModelsView
{
    public class RegisterViewModel
    {
        [Key]
        public int? MaDg { get; set; }
        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage ="Vui lòng nhập Họ Tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Số Điện Thoại")]
        [Display(Name ="Điện Thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidatePhone", controller:"Accounts")]
        public int Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [Display(Name = "Email")]
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail", controller: "Accounts")]
        public string Email { get; set; }
        [Display(Name ="Mật Khẩu")]
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [MinLength(5,ErrorMessage ="Bạn phải nhập tối thiểu 5 ký tự")]
        public string Password { get; set; }
        [MinLength(5,ErrorMessage ="Bạn phải nhập tối thiểu 5 ký tự")]
        [Display(Name ="Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Vui lòng nhập mật khẩu giống nhau")]
        public string ConfirmPassword { get; set; }

    }
}
