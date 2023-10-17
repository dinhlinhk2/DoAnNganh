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
    [Authorize]
    public class AdminTheLoaisController : Controller
    {
        private readonly WebBanSachOnlContext _context;
        public INotyfService thongBao { get; }

        public AdminTheLoaisController(WebBanSachOnlContext context, INotyfService thongBao)
        {
            _context = context;
            this.thongBao = thongBao;
        }
        //Phân Trang
        public IActionResult Index(int? page)
        {
            var soPage = page == null || page <= 0 ? 1 : page.Value;
            var soTrang = 15;
            var IsKH = _context.TheLoais.AsNoTracking().OrderByDescending(x => x.MaTl);
            PagedList<TheLoai> models = new PagedList<TheLoai>(IsKH, soPage, soTrang);
            ViewBag.CurrentPage = soPage;
            return View(models);
        }

        /*// GET: Admin/AdminTheLoais
        public async Task<IActionResult> Index()
        {
              return _context.TheLoais != null ? 
                          View(await _context.TheLoais.ToListAsync()) :
                          Problem("Entity set 'WebBanSachOnlContext.TheLoais'  is null.");
        }*/

        // GET: Admin/AdminTheLoais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TheLoais == null)
            {
                return NotFound();
            }

            var theLoai = await _context.TheLoais
                .FirstOrDefaultAsync(m => m.MaTl == id);
            if (theLoai == null)
            {
                return NotFound();
            }

            return View(theLoai);
        }

        // GET: Admin/AdminTheLoais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminTheLoais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTl,TenTl")] TheLoai theLoai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theLoai);
                await _context.SaveChangesAsync();
                thongBao.Success("Tạo Thành Công");
                return RedirectToAction(nameof(Index));
            }
            return View(theLoai);
        }

        // GET: Admin/AdminTheLoais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TheLoais == null)
            {
                return NotFound();
            }

            var theLoai = await _context.TheLoais.FindAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }
            return View(theLoai);
        }

        // POST: Admin/AdminTheLoais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTl,TenTl")] TheLoai theLoai)
        {
            if (id != theLoai.MaTl)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theLoai);
                    await _context.SaveChangesAsync();
                    thongBao.Success("Sửa Thành Công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheLoaiExists(theLoai.MaTl))
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
            return View(theLoai);
        }

        // GET: Admin/AdminTheLoais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TheLoais == null)
            {
                return NotFound();
            }

            var theLoai = await _context.TheLoais
                .FirstOrDefaultAsync(m => m.MaTl == id);
            if (theLoai == null)
            {
                return NotFound();
            }

            return View(theLoai);
        }

        // POST: Admin/AdminTheLoais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TheLoais == null)
            {
                return Problem("Entity set 'WebBanSachOnlContext.TheLoais'  is null.");
            }
            var theLoai = await _context.TheLoais.FindAsync(id);
            if (theLoai != null)
            {
                _context.TheLoais.Remove(theLoai);
            }
            
            await _context.SaveChangesAsync();
            thongBao.Success("Xóa Thành Công");
            return RedirectToAction(nameof(Index));
        }

        private bool TheLoaiExists(int id)
        {
          return (_context.TheLoais?.Any(e => e.MaTl == id)).GetValueOrDefault();
        }
    }
}
