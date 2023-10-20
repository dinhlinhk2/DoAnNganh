using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Extension;
using Demo.Helpper;
using Demo.Models;
using Demo.ModelsView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Demo.Controllers
{
    /*[Authorize]*/
    public class AccountsController : Controller
    {
        private readonly WebBanSachOnlContext _context;
        public INotyfService thongBao { get; }
        public AccountsController(WebBanSachOnlContext context, INotyfService notyfService )
        {
            _context = context;
            thongBao = notyfService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(int Phone)
        {
            try
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Sdt == Phone);
                if (khachhang != null)
                    return Json(data: "Số điện thoại:" + Phone + "Đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email:" + Email + "Đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("MaDg");
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs
                    .AsNoTracking().SingleOrDefault(x => x.MaDg == Convert.ToInt32(taikhoanID));
                if(khachhang != null)
                {
                    var lsDonHang = _context.HoaDons
                        .Include(x => x.TrangThai)
                        .AsNoTracking().Where(x => x.MaDg == khachhang.MaDg)
                        .OrderByDescending(x=> x.NgayLapHd).ToList();
                    ViewBag.DonHang = lsDonHang;
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html",Name = "DangKy")]
        public IActionResult DangKyTaiKhoan()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name ="DangKy")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterViewModel taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    KhachHang khachhang = new KhachHang
                    {

                        TenDg = taikhoan.FullName,
                        Email = taikhoan.Email.Trim().ToLower(),
                        Sdt = taikhoan.Phone,
                        MatKhau = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Active = true,
                        Salt = salt,
                        NgayTao = DateTime.Now
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("MaDg", khachhang.MaDg.ToString());
                        var taikhoanID = HttpContext.Session.GetString("MaDg");
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.TenDg),
                            new Claim("MaDg", khachhang.MaDg.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        thongBao.Success("thành công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");
                    }
                     
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }

        }

		[HttpGet]
        [AllowAnonymous]
        [Route("dang-nhap.html",Name ="DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaDg");
            if(taikhoanID != null)
            {
                
                return RedirectToAction("Dashboard", "Accounts");

            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel customer)
        {
            try
            {
				if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);
                    var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);
                    if (khachhang == null) return RedirectToAction("DangKyTaiKhoan");

                    string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();
                    if (khachhang.MatKhau != pass)
                    {
                        thongBao.Success("Nhập sai mật khẩu");
                        return View(customer);
                    }
                    if (khachhang.Active == false)
                    {
                        return RedirectToAction("False", "Accounts");
                    }
                    HttpContext.Session.SetString("MaDg", khachhang.MaDg.ToString());
                    var taikhoanID = HttpContext.Session.GetString("MaDg");
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.TenDg),
                            new Claim("MaDg", khachhang.MaDg.ToString())
                        };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    thongBao.Success("Đăng Nhập Thành Công");
                    return RedirectToAction("Dashboard", "Accounts");
                }
               
            }
            catch (Exception ex)
            {
                return RedirectToAction("DangKyTaiKhoan", "Accounts");
            }
            return View(customer);
        }

		[HttpGet]
		[Route("dang-xuat.html", Name = "DangXuat")]
        public IActionResult Logout()
		{
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("MaDg");
            return RedirectToAction("Index", "Home");
		}

		[HttpPost]
        public IActionResult ChangePassword(ThayDoiPassViewModel model)
		{
			try
			{
                var taikhoanID = HttpContext.Session.GetString("MaDg");
                if (taikhoanID ==null)
				{
                    return RedirectToAction("Login", "Accounts");
				}
                if (ModelState.IsValid)
				{
                    var taikhoan = _context.KhachHangs.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null)
                    {
                        return RedirectToAction("Login", "Accounts");
                    }
                    var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    if (pass == taikhoan.MatKhau)
                    {
                        string newPass = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                        taikhoan.MatKhau = newPass;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        thongBao.Success("Thay Mật Khẩu Thành Công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
				}
            }
			catch
			{
                thongBao.Success("Thay Mật Khẩu Không Thành Công");
                return RedirectToAction("Dashboard", "Accounts");
            }
            thongBao.Success("Thay Mật Khẩu Không Thành Công");
            return RedirectToAction("Dashboard", "Accounts");

        }

    }
}
