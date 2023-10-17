using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Extension;
using Demo.Models;
using Demo.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly WebBanSachOnlContext _context;
        public INotyfService thongBao { get; }
        public CheckoutController(WebBanSachOnlContext context, INotyfService notyfService)
        {
            _context = context;
            thongBao = notyfService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaDg");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaDg == Convert.ToInt32(taikhoanID));
                model.MaDg = khachhang.MaDg;
                model.FullName = khachhang.TenDg;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Sdt.ToString();
                model.Address = khachhang.DiaChi;
            }
            ViewBag.GioHang = cart;
            return View(model);
        }

        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(MuaHangVM muaHang)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaDg");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaDg == Convert.ToInt32(taikhoanID));
                model.MaDg = khachhang.MaDg;
                model.FullName = khachhang.TenDg;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Sdt.ToString();
                model.Address = khachhang.DiaChi;

                khachhang.DiaChi = muaHang.Address;
                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                HoaDon donhang = new HoaDon();
                donhang.MaDg = model.MaDg;
                donhang.NgayLapHd = DateTime.Now;
                donhang.TongGiaTri = Convert.ToInt32(cart.Sum(x => x.TongTien));

                _context.Add(donhang);
                _context.SaveChanges();

                foreach (var item in cart)
                {
                    ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
                    chiTietHoaDon.MaHd = donhang.MaHd;
                    chiTietHoaDon.MaTs = item.product.MaTs;
                    chiTietHoaDon.SoLuong = item.amount;
                    chiTietHoaDon.ThanhTien = donhang.TongGiaTri;
                    chiTietHoaDon.DonGia = (int)item.product.Gia;
                    _context.Add(chiTietHoaDon);

                }
                _context.SaveChanges();
                HttpContext.Session.Remove("GioHang");
                thongBao.Success("Đặt Hàng Thành Công");
                return RedirectToAction("Success");

            }
            catch (Exception ex)
            {
                ViewBag.GioHang = cart;
                return View(model);
            }
            ViewBag.GioHang = cart;
            return View(model);
        }

        [Route("dat-hang-thanh-cong.html", Name = "Success")]
        public IActionResult Success()
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("MaDg");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });

                }
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaDg == Convert.ToInt32(taikhoanID));
                var donhang = _context.HoaDons.Where(x => x.MaDg == Convert.ToInt32(taikhoanID))
                    .OrderByDescending(x => x.NgayLapHd).ToList();
                MuaHangSuccessVM successVM = new MuaHangSuccessVM();
                successVM.FullName = khachhang.TenDg;
                successVM.Phone = khachhang.Sdt.ToString();
                successVM.Address = khachhang.DiaChi;
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }
    }
}
