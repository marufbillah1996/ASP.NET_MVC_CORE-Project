using Microsoft.AspNetCore.Mvc;

namespace Single_Page_Application.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
