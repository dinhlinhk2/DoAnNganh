using System.ComponentModel.DataAnnotations;

namespace Demo.ModelsView
{
	public class ThayDoiPassViewModel
	{
		[Key]
		public int MaDg { get; set; }
		[Display(Name ="Mật Khẩu Hiện Tại")]
		public string PasswordNow { get; set; }
		[Display(Name ="Mật Khẩu Mới")]
		[Required(ErrorMessage ="Vui Lòng Nhập Mật Khẩu")]
		[MinLength(5,ErrorMessage ="Bạn Phải Nhập Tối Thiểu 5 Ký Tự")]
		public string Password { get; set; }
		[MinLength(5,ErrorMessage ="Bản Phải Nhập Tối Thiểu 5 Ký Tự")]
		[Display(Name ="Nhập Lại Mật Khẩu")]
		[Compare("Password", ErrorMessage ="Mật Khẩu Không Giống Nhau")]
		public string ConfirmPassword { get; set; }

	}
}
