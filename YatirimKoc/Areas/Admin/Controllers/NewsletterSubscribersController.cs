using Microsoft.AspNetCore.Mvc;

namespace YatirimKoc.Web.Areas.Admin.Controllers
{
    public class NewsletterSubscribersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
