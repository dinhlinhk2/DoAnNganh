using Demo.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin.html", Name = "AdminIndex")]
    [Authorize(Roles ="Admin,Staff")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                return View("Index");
            }
            return RedirectToAction("AdminLogin", "Accounts", new { Area = "Admin" });
        }
    }
}
