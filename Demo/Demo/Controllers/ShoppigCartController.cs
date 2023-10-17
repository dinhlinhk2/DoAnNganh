using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Extension;
using Demo.Models;
using Demo.ModelsView;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
	public class ShoppigCartController : Controller
	{
		private readonly WebBanSachOnlContext _context;
		public INotyfService thongBao { get; }
		public ShoppigCartController(WebBanSachOnlContext context, INotyfService notyfService)
		{
			_context = context;
			thongBao = notyfService;
		}
		public List<CartItem> GioHang
		{
			get
			{
				var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
				if (gh == default(List<CartItem>))
				{
					gh = new List<CartItem>();
				}
				return gh;
			}
		}
		[HttpPost]
		[Route("api/cart/add")]
		public IActionResult AddToCart(int productID, int? amount)
		{
			List<CartItem> giohang = GioHang;
			try
			{
				CartItem item = GioHang.SingleOrDefault(p => p.product.MaTs == productID);
				if (item != null)
				{
					item.amount = item.amount + amount.Value;
					HttpContext.Session.Set<List<CartItem>>("GioHang", giohang);
				}
				else
				{
					Sach hh = _context.Saches.SingleOrDefault(p => p.MaTs == productID);
					item = new CartItem
					{
						amount = amount.HasValue ? amount.Value : 1,
						product = hh
					};
					giohang.Add(item); // thêm vào giỏ hàng
				}
				// lưu vào giỏ hàng
				HttpContext.Session.Set<List<CartItem>>("GioHang", giohang);
				thongBao.Success("Thêm Sản Phẩm Thành Công");
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
			/*return Json(new { success = true });*/
			
		}

        [HttpPost]
        [Route("api/cart/update")]
		public IActionResult UpdateCart(int productID, int? amount)
        {
			var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if (cart != null)
                {
					CartItem item = cart.SingleOrDefault(p => p.product.MaTs == productID);
					if(item != null && amount.HasValue)
                    {
						item.amount = amount.Value;
                    }
					HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
				return Json(new { success = true });
            }
            catch
            {
				return Json(new { success = false });

			}
		}

		[HttpPost]
		[Route("api/cart/remove")]
		public ActionResult Remove(int productID)
		{
			try
			{
				List<CartItem> giohang = GioHang;
				CartItem item = giohang.SingleOrDefault(p => p.product.MaTs == productID);
				if (item != null)
				{
					giohang.Remove(item);
				}
				// lưu lại giỏ hàng
				HttpContext.Session.Set<List<CartItem>>("GioHang", giohang);
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}

		[Route("cart.html", Name ="Cart")]
		public IActionResult Index()
		{
			/*List<int> lsProductIDs = new List<int>();*/
			return View(GioHang);
		}
	}
}
