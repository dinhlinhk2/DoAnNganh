using Demo.Models;
using Demo.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebBanSachOnlContext _banSachOnlContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, WebBanSachOnlContext banSachOnlContext)
        {
            _logger = logger;
            _banSachOnlContext = banSachOnlContext;
        }

        public IActionResult Index()
        {
            HomeViewMV model = new HomeViewMV();
            var lsProducts = _banSachOnlContext.Saches.AsNoTracking()
                .OrderByDescending(x => x.NamXb)
                .ToList();

            List<ProductHomeMV> productHomeMVs = new List<ProductHomeMV>();

            var lsCat = _banSachOnlContext.TheLoais
                .AsNoTracking()
                .OrderByDescending(x => x.TenTl)
                .ToList();

            foreach (var item in lsCat)
            {
                ProductHomeMV productHomeMV = new ProductHomeMV();
                productHomeMV.TheLoai = item;
                productHomeMV.lsProducts = lsProducts.Where(x => x.MaTl == item.MaTl).ToList();
                productHomeMVs.Add(productHomeMV);
            }
            model.Products = productHomeMVs;
            ViewBag.AllSach = lsProducts;
            return View(model);
        }
        [Route("lien-he.html", Name ="Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("gioi-thieu.html",Name ="About")]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}