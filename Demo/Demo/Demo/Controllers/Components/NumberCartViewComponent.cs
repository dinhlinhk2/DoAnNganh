using Demo.Extension;
using Demo.ModelsView;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers.Components
{
	public class NumberCartViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
			return View(cart);
		}
	}
}
