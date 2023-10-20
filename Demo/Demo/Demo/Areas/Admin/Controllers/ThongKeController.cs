using Demo.Models;
using Microsoft.AspNetCore.Mvc;
namespace Demo.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly WebBanSachOnlContext _context;
     
        public ThongKeController(WebBanSachOnlContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in _context.HoaDons
                        join od in _context.ChiTietHoaDons on o.MaHd equals od.MaCthd
                        join p in _context.Saches on od.MaTs equals p.MaTs
                        select new
                        {
                            NgayLap_HD = o.NgayLapHd,
                            SoLuong = od.SoLuong,
                            Gia = od.DonGia,
                            DonGia = p.Gia
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.NgayLap_HD >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.NgayLap_HD < endDate);

            }
            var result = query.GroupBy(x => x.NgayLap_HD).Select(x => new
            {
                Date = x.Key,
                TotalBuy = x.Sum(y => y.SoLuong * y.DonGia),
                TotalSell = x.Sum(y => y.SoLuong * y.Gia)

            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new {Data = result});
        }

    }
}
