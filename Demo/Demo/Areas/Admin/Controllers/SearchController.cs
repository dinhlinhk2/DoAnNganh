using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly WebBanSachOnlContext _context;
        public INotyfService thongBao { get; }
        public SearchController(WebBanSachOnlContext context, INotyfService notyf)
        {
            _context = context;
            thongBao = notyf;
        }

        [HttpPost]
        public IActionResult findSP(string kw)
        {
            List<Sach> Is = new List<Sach>();
            if(string.IsNullOrEmpty(kw) || kw.Length < 1)
            {
                return PartialView("ListSPSearchPartial", null);

            }
            Is = _context.Saches
                .AsNoTracking()
                .Include(x => x.MaTlNavigation)
                .Where(x => x.TenTs.Contains(kw))
                .OrderByDescending(x => x.TenTs)
                .Take(10)
                .ToList();
            if (Is == null)
            {
                return PartialView("ListSPSearchPartial", null);
            }
            else
                return PartialView("ListSPSearchPartial", Is);
        }

        [HttpPost]
        public IActionResult findKH(string kw)
        {
            List<KhachHang> Is = new List<KhachHang>();
            if (string.IsNullOrEmpty(kw) || kw.Length < 1)
            {
                return PartialView("ListKHSearchPartial", null);

            }
            Is = _context.KhachHangs
                .AsNoTracking()
                .Where(x => x.TenDg.Contains(kw))
                .OrderByDescending(x => x.TenDg)
                .Take(10)
                .ToList();
            if (Is == null)
            {
                return PartialView("ListKHSearchPartial", null);
            }
            else
                return PartialView("ListKHSearchPartial", Is);
        }
    }
}
