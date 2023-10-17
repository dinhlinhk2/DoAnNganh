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
using Demo.Extension;
using Demo.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly WebBanSachOnlContext _context;
        public INotyfService thongBao { get; }

        public AdminAccountsController(WebBanSachOnlContext context, INotyfService notyf)
        {
            _context = context;
            thongBao = notyf;
        }

        // GET: Admin/AdminAccounts
        /*public async Task<IActionResult> Index()
        {
            var webBanSachOnlContext = _context.Accounts.Include(a => a.Role);
            return View(await webBanSachOnlContext.ToListAsync());
        }*/
        public IActionResult Index(int page = 1, int idRole = 0)
        {
            var soPage = page;
            var soTrang = 15;
            List<Account> IsAC = new List<Account>();
            if (idRole != 0)
            {
                IsAC = _context.Accounts
                    .AsNoTracking()
                    .Where(x => x.RoleId == idRole)
                    .Include(x => x.Role)
                    .OrderByDescending(x => x.AccountId).ToList();
            }
            else
            {
                IsAC = _context.Accounts
                    .AsNoTracking()
                    .Include(x => x.Role)
                    .OrderByDescending(x => x.AccountId).ToList();
            }
            PagedList<Account> models = new PagedList<Account>(IsAC.AsQueryable(), soPage, soTrang);
            ViewBag.CurrentRoleId = idRole;
            ViewBag.CurrentPage = soPage;
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "Description");
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", idRole);
            return View(models);
        }
        public IActionResult Filter(int idRole = 0)
        {
            var url = $"/Admin/AdminAccounts?idRole={idRole}";
            if (idRole == 0)
            {
                url = $"/Admin/AdminAccounts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/AdminAccounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Email,Phone,MatKhau,RoleId")] Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    account.Salt = salt;
                    account.MatKhau = (account.Phone + salt.Trim()).ToMD5();
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                    thongBao.Success("Tạo Thành Công");
                    return RedirectToAction(nameof(Index));
                }
                ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
                return View(account);
            }
            catch (Exception ex)
            {

            }
            _context.Add(account);
            await _context.SaveChangesAsync();
            thongBao.Success("Tạo Thành Công");
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/AdminAccounts/ChangePass
        public IActionResult ChangePass()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        // GET: Admin/AdminAccounts/ChangePass
        [HttpPost]
        public IActionResult ChangePass(ThayDoiPassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taikhoan = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Email == model.Email);
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
                    return RedirectToAction("Login", "Accounts", new { Area = "Admin"});
                }
            }
            return View();
        }

        // GET: Admin/AdminAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Email,Phone,MatKhau,RoleId")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                    thongBao.Success("Cập Nhật Thành Công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // GET: Admin/AdminAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'WebBanSachOnlContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            thongBao.Success("Xóa Thành Công");
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
          return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
