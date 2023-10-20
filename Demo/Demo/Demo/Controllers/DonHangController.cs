using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Models;
using Demo.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
	public class DonHangController : Controller
	{
		private readonly WebBanSachOnlContext _context;
		public INotyfService thongBao { get; }
		public DonHangController(WebBanSachOnlContext context, INotyfService notyfService)
		{
			_context = context;
			thongBao = notyfService;
		}

		[HttpPost]
		[Route("DonHang/Details")]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) 
			{ 
				return NotFound(); 
			}
			try
			{
				var taikhoanID = HttpContext.Session.GetString("MaDg");
				if (string.IsNullOrEmpty(taikhoanID)) return RedirectToAction("Login", "Accounts");
				var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaDg == Convert.ToInt32(taikhoanID));
				if (khachhang == null) return NotFound();
				var donhang = await _context.HoaDons
					.Include(x=>x.TrangThai)
					.FirstOrDefaultAsync(x => x.MaHd == id && Convert.ToInt32(taikhoanID) == x.MaDg);
				if (donhang == null) return NotFound();

				var chitietDH = _context.ChiTietHoaDons
					.Include(x=>x.MaTsNavigation)
					.AsNoTracking()
					.Where(x => x.MaHd == id)
					.OrderBy(x => x.MaCthd)
					.ToList();
				XemDonHang donHang = new XemDonHang();
				donHang.DonHang = donhang;
				donHang.ChiTietDonHang = chitietDH;
				ViewBag.ChiTiet = chitietDH;

				return PartialView("Details", donHang);

			}
			catch
			{
				return NotFound();
			}

		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
