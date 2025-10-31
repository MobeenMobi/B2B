using Microsoft.AspNetCore.Mvc;

namespace B2B.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
