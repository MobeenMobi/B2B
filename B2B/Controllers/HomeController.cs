using System.Diagnostics;
using B2B.Data;
using B2B.Models;
using Microsoft.AspNetCore.Mvc;

namespace B2B.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

                if (user == null)
                {
                    ViewBag.Error = "Invalid email or password.";
                    return View();
                }
                else
                {
                    return RedirectToAction("Dashboard");
                }
                   
            }
            else
            {
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Dashboard()
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
