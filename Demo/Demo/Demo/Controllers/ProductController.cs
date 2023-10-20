using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Demo.Controllers
{
   
    public class ProductController : Controller
    {
		private readonly WebBanSachOnlContext _context;
		public ProductController(WebBanSachOnlContext context)
		{
			_context = context;
		}
        /*[Route("shop.html", Name = "SanPham")]*/
        public IActionResult Shop(int? page, int TLid=0)
		{
            try
            {
				var soPage = page == null || page <= 0 ? 1 : page.Value;
				var soTrang = 10;
				List<Sach> IsSaches = new List<Sach>();
				if (TLid != 0)
				{
					IsSaches = _context.Saches
						.AsNoTracking()
						.Where(x => x.MaTl == TLid)
						.Include(x => x.MaTlNavigation)
						.OrderByDescending(x => x.MaTs).ToList();
				}
				else
				{
					IsSaches = _context.Saches
						.AsNoTracking()
						.Include(x => x.MaTlNavigation)
						.OrderByDescending(x => x.MaTs).ToList();
				}
				PagedList<Sach> models = new PagedList<Sach>(IsSaches.AsQueryable(), soPage, soTrang);
				ViewData["TheLoai"] = new SelectList(_context.TheLoais, "MaTl", "TenTl");
				ViewBag.CurrentPage = soPage;
				return View(models);
			}
            catch
            {
				ViewData["TheLoai"] = new SelectList(_context.TheLoais, "MaTl", "TenTl");
				return RedirectToAction("Index", "Home");
			}
			
		}
		public IActionResult Filter(int TLid = 0)
		{
			var url = $"/Product/Shop?TLid={TLid}";
			if (TLid == 0)
			{
				url = $"/Product/Shop";
			}
			return Json(new { status = "success", redirectUrl = url });
		}

		[Route("/{Alias}", Name = "ListProduct")]
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
				var soTrang = 10;
				var theloai = _context.TheLoais.AsNoTracking().SingleOrDefault(x => x.TenTl == Alias);
				var IsKH = _context.Saches
					.AsNoTracking()
					.Where(x => x.MaTl == theloai.MaTl)
					.OrderByDescending(x => x.NamXb);
				PagedList<Sach> models = new PagedList<Sach>(IsKH, page, soTrang);
				ViewBag.CurrentPage = page;
				ViewBag.CurrentTL = theloai;
				return View(models);
			}
            catch
            {
				return RedirectToAction("Index", "Home");
			}
			
        }
        [Route("/{TenTs}-{id}.html", Name = "ProductDetails")]
        public IActionResult Details(int id)
		{
			try
			{
				var pro = _context.Saches.Include(x => x.MaTlNavigation).FirstOrDefault(x => x.MaTs == id);
				if (pro == null)
				{
					return RedirectToAction("Index");
				}
				var lsProduct = _context.Saches.AsNoTracking()
					.Where(x => x.MaTl == pro.MaTl && x.MaTs != id && x.SoLuong > 0)
					.Take(4)
					.OrderByDescending(x => x.NamXb)
					.ToList();
				ViewBag.SanPham = lsProduct;
				return View(pro);
			}
			catch
			{
                return RedirectToAction("Index", "Home");

            }
		}

		public async Task<IActionResult> LocSP(int idTL)
        {
			var pro = _context.Saches.Where(p => p.MaTl == idTL).Include(p => p.MaTlNavigation);
			return View("Index",await pro.ToListAsync());
        }
	}
}
