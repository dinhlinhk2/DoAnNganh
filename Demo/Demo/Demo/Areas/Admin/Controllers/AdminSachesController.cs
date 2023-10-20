using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using X.PagedList;
using Demo.Helpper;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class AdminSachesController : Controller
    {
        private readonly WebBanSachOnlContext _context;
        public INotyfService thongBao { get; }
        public AdminSachesController(WebBanSachOnlContext context, INotyfService notyf)
        {
            _context = context;
            thongBao = notyf;
        }

        // GET: Admin/AdminSaches
        /*public async Task<IActionResult> Index()
        {
            var webBanSachOnlContext = _context.Saches.Include(s => s.MaKhoNavigation).Include(s => s.MaNsxNavigation).Include(s => s.MaTlNavigation);
            return View(await webBanSachOnlContext.ToListAsync());
        }*/
        public IActionResult Index(int page = 1, int TLid = 0)
        {
            var soPage = page;
            var soTrang = 15;
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
            ViewBag.CurrentMaTL = TLid;
            ViewBag.CurrentPage = soPage;
            ViewData["TheLoai"] = new SelectList(_context.TheLoais, "MaTl", "TenTl");
            ViewData["DanhMuc"] = new SelectList(_context.TheLoais, "MaTl", "TenTl", TLid);
            return View(models);
        }
        public IActionResult Filter(int TLid = 0)
        {
            var url = $"/Admin/AdminSaches?TLid={TLid}";
            if (TLid == 0)
            {
                url = $"/Admin/AdminSaches";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminSaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Saches == null)
            {
                return NotFound();
            }

            var sach = await _context.Saches
                .Include(s => s.MaKhoNavigation)
                .Include(s => s.MaNsxNavigation)
                .Include(s => s.MaTlNavigation)
                .FirstOrDefaultAsync(m => m.MaTs == id);
            if (sach == null)
            {
                return NotFound();
            }

            return View(sach);
        }

        // GET: Admin/AdminSaches/Create
        public IActionResult Create()
        {
            ViewData["MaKho"] = new SelectList(_context.Khos, "MaKho", "TenSach");
            ViewData["MaNsx"] = new SelectList(_context.Nsxes, "MaNsx", "TenNsx");
            ViewData["DanhMuc"] = new SelectList(_context.TheLoais, "MaTl", "TenTl");
            return View();
        }

        // POST: Admin/AdminSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTs,TenTs,MaTl,Gia,SoLuong,NamXb,MaNsx,MaKho,Anh")] Sach sach, Microsoft.AspNetCore.Http.IFormFile fileAnh)
        {
            if (ModelState.IsValid)
            {
                sach.TenTs = Utilities.ToTitleCase(sach.TenTs);
                if (fileAnh != null)
                {
                    string extension = Path.GetExtension(fileAnh.FileName);
                    string img = Utilities.SEOUrl(sach.TenTs) + extension;
                    sach.Anh = await Utilities.UploadFile(fileAnh, @"saches", img.ToLower());
                }
                if (string.IsNullOrEmpty(sach.Anh)) sach.Anh = "default.jpg";
                sach.NamXb = DateTime.Now;
                _context.Add(sach);
                await _context.SaveChangesAsync();
                thongBao.Success("Tạo Thành Công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKho"] = new SelectList(_context.Khos, "MaKho", "TenSach", sach.MaKho);
            ViewData["MaNsx"] = new SelectList(_context.Nsxes, "MaNsx", "TenNsx", sach.MaNsx);
            ViewData["DanhMuc"] = new SelectList(_context.TheLoais, "MaTl", "TenTl", sach.MaTl);
            return View(sach);
        }

        // GET: Admin/AdminSaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Saches == null)
            {
                return NotFound();
            }

            var sach = await _context.Saches.FindAsync(id);
            if (sach == null)
            {
                return NotFound();
            }
            ViewData["MaKho"] = new SelectList(_context.Khos, "MaKho", "TenSach", sach.MaKho);
            ViewData["MaNsx"] = new SelectList(_context.Nsxes, "MaNsx", "TenNsx", sach.MaNsx);
            ViewData["DanhMuc"] = new SelectList(_context.TheLoais, "MaTl", "TenTl", sach.MaTl);
            return View(sach);
        }

        // POST: Admin/AdminSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTs,TenTs,MaTl,Gia,SoLuong,NamXb,MaNsx,MaKho,Anh")] Sach sach, Microsoft.AspNetCore.Http.IFormFile fileAnh)
        {
            if (id != sach.MaTs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sach.TenTs = Utilities.ToTitleCase(sach.TenTs);
                    if (fileAnh != null)
                    {
                        string extension = Path.GetExtension(fileAnh.FileName);
                        string img = Utilities.SEOUrl(sach.TenTs) + extension;
                        sach.Anh = await Utilities.UploadFile(fileAnh, @"saches", img.ToLower());
                    }
                    if (string.IsNullOrEmpty(sach.Anh)) sach.Anh = "default.jpg";
                    sach.NamXb = DateTime.Now;
                    _context.Update(sach);
                    await _context.SaveChangesAsync();
                    thongBao.Success("Cập Nhật Thành Công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SachExists(sach.MaTs))
                    {
                        thongBao.Success("Có Lỗi!");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKho"] = new SelectList(_context.Khos, "MaKho", "TenSach", sach.MaKho);
            ViewData["MaNsx"] = new SelectList(_context.Nsxes, "MaNsx", "TenNsx", sach.MaNsx);
            ViewData["DanhMuc"] = new SelectList(_context.TheLoais, "MaTl", "TenTl", sach.MaTl);
            return View(sach);
        }

        // GET: Admin/AdminSaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Saches == null)
            {
                return NotFound();
            }

            var sach = await _context.Saches
                .Include(s => s.MaKhoNavigation)
                .Include(s => s.MaNsxNavigation)
                .Include(s => s.MaTlNavigation)
                .FirstOrDefaultAsync(m => m.MaTs == id);
            if (sach == null)
            {
                return NotFound();
            }

            return View(sach);
        }

        // POST: Admin/AdminSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Saches == null)
            {
                return Problem("Entity set 'WebBanSachOnlContext.Saches'  is null.");
            }
            var sach = await _context.Saches.FindAsync(id);
            if (sach != null)
            {
                _context.Saches.Remove(sach);
            }
            
            await _context.SaveChangesAsync();
            thongBao.Success("Xóa Thành Công");
            return RedirectToAction(nameof(Index));
        }

        private bool SachExists(int id)
        {
          return (_context.Saches?.Any(e => e.MaTs == id)).GetValueOrDefault();
        }
    }
}
