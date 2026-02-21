using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YatirimKoc.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // Login olmadan erişilemez
    [Route("admin/[controller]/[action]")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Genel Bakış";
            return View();
        }
    }
}
