using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using B2B.Data;
using B2B.EmailService;
using B2B.Models;
using B2B.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace B2B.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IEmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(string email, string password)
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
                    string OTP =  GenerateSecureOtp();
                    TempData["OTP"] = OTP;
                    TempData["OtpExpiry"] = DateTime.Now.AddMinutes(5);
                    //await SendOtp(user.Email, OTP);


                    HttpContext.Session.SetString("UserName", user.FullName);

                    // ✅ Create claims (store user info here)
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),   // OR user.Email
                new Claim(ClaimTypes.Email, user.Email)
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // ✅ Sign in (set cookie)
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("VerifyOtp", "Home");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var companyName = model.Company.CompanyName;
            var bankName = model.Bank.Bank;
            var userFirstName = model.User.FirstName;


            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); // or your controller name
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SendOtp(string clientEmail, string otp)
        {
            await _emailService.SendEmailAsync(clientEmail, "Your OTP", otp);
            return Ok("Email Sent");
        }

        public string GenerateSecureOtp()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                int otp = BitConverter.ToInt32(bytes, 0) % 1000000;
                return Math.Abs(otp).ToString("D6"); // Always 6 digits
            }
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            string storedOtp = TempData["OTP"]?.ToString();
            DateTime expiry = Convert.ToDateTime(TempData["OtpExpiry"]);

            if (storedOtp == null || DateTime.Now > expiry)
                return Content("OTP expired. Please request again.");

            if (otp == storedOtp)
                return RedirectToAction("Dashboard");

            TempData.Keep(); // keep OTP alive for re-entry
            return Content("Invalid OTP. Try again.");
        }
    }
}
