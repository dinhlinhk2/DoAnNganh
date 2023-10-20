using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Models;
using X.PagedList;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class AdminKhachHangsController : Controller
    {
        private readonly WebBanSachOnlContext _context;
        public INotyfService thongBao { get; }

        public AdminKhachHangsController(WebBanSachOnlContext context, INotyfService notyfService)
        {
            _context = context;
            thongBao = notyfService;
        }


        //Phân Trang
        public IActionResult Index(int? page)
        {
            var soPage = page == null || page <= 0 ? 1 : page.Value;
            var soTrang = 15;
            var IsKH = _context.KhachHangs.AsNoTracking().OrderByDescending(x => x.NgayTao);
            PagedList<KhachHang> models = new PagedList<KhachHang>(IsKH, soPage, soTrang);
            ViewBag.CurrentPage = soPage;
            return View(models);
        }

        // GET: Admin/AdminKhachHangs
       /* public async Task<IActionResult> Index()
        {
              return _context.KhachHangs != null ? 
                          View(await _context.KhachHangs.ToListAsync()) :
                          Problem("Entity set 'WebBanSachOnlContext.KhachHangs'  is null.");
        }*/

        // GET: Admin/AdminKhachHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaDg == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Admin/AdminKhachHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminKhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDg,TenDg,DiaChi,Sdt,GioiTinh,NgaySinh,TaiKhoan,MatKhau,NgayTao")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: Admin/AdminKhachHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/AdminKhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDg,TenDg,DiaChi,Sdt,GioiTinh,NgaySinh,TaiKhoan,MatKhau,NgayTao")] KhachHang khachHang)
        {
            if (id != khachHang.MaDg)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                    thongBao.Success("Cập Nhật Thành Công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaDg))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: Admin/AdminKhachHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaDg == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: Admin/AdminKhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KhachHangs == null)
            {
                return Problem("Entity set 'WebBanSachOnlContext.KhachHangs'  is null.");
            }
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
            }
            
            await _context.SaveChangesAsync();
            thongBao.Success("Xóa Thành Công");
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
          return (_context.KhachHangs?.Any(e => e.MaDg == id)).GetValueOrDefault();
        }
    }
}
