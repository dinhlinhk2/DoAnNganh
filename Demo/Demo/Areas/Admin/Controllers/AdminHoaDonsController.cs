using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Models;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminHoaDonsController : Controller
    {
        private readonly WebBanSachOnlContext _context;

        public AdminHoaDonsController(WebBanSachOnlContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminHoaDons
        public IActionResult Index(int? page)
        {
            var soPage = page == null || page <= 0 ? 1 : page.Value;
            var soTrang = 15;
            var IsHoaDons = _context.HoaDons.Include(h => h.MaDgNavigation).AsNoTracking().OrderByDescending(x => x.NgayLapHd);
            PagedList<HoaDon> models = new PagedList<HoaDon>(IsHoaDons, soPage, soTrang);
            ViewBag.CurrentPage = soPage;
            return View(models);
        }

        // GET: Admin/AdminHoaDons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaDgNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            var chitietHD = _context.ChiTietHoaDons.AsNoTracking()
                .Include(x=>x.MaTsNavigation)
                .Where(x => x.MaHd == hoaDon.MaHd).OrderBy(x => x.MaCthd).ToList();
            ViewBag.ChiTiet = chitietHD;
            return View(hoaDon);
        }

        // GET: Admin/AdminHoaDons/Create
        public IActionResult Create()
        {
            ViewData["MaDg"] = new SelectList(_context.KhachHangs, "MaDg", "MaDg");
            return View();
        }

        // POST: Admin/AdminHoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHd,NgayLapHd,TongGiaTri,MaDg")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDg"] = new SelectList(_context.KhachHangs, "MaDg", "MaDg", hoaDon.MaDg);
            return View(hoaDon);
        }

        // GET: Admin/AdminHoaDons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            ViewData["MaDg"] = new SelectList(_context.KhachHangs, "MaDg", "MaDg", hoaDon.MaDg);
            return View(hoaDon);
        }

        // POST: Admin/AdminHoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHd,NgayLapHd,TongGiaTri,MaDg")] HoaDon hoaDon)
        {
            if (id != hoaDon.MaHd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.MaHd))
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
            ViewData["MaDg"] = new SelectList(_context.KhachHangs, "MaDg", "MaDg", hoaDon.MaDg);
            return View(hoaDon);
        }

        // GET: Admin/AdminHoaDons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaDgNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: Admin/AdminHoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HoaDons == null)
            {
                return Problem("Entity set 'WebBanSachOnlContext.HoaDons'  is null.");
            }
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
          return (_context.HoaDons?.Any(e => e.MaHd == id)).GetValueOrDefault();
        }
    }
}
