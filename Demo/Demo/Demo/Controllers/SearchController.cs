using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
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
            if (string.IsNullOrEmpty(kw) || kw.Length < 1)
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
    }
}
