using B2B.Data;
using B2B.EmailService;
using B2B.Models;
using B2B.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace B2B.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public LoginController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return View();

            var user = _context.UserLogins.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                
                return View();
            }


            var Firstname = _context.Users.Where(u => u.Id == user.UserId).Select(u => u.FirstName).FirstOrDefault();
            bool KYBStatus = _context.Users.Where(u => u.Id == user.UserId).Select(u => u.IsKYBApproved).FirstOrDefault();

            if (!KYBStatus)
            {
                ViewBag.Error = "KYB not approved yet. Please wait for approval.";
                return View();
            }
            string otp = GenerateSecureOtp();
            TempData["OTP"] = otp;
            TempData["OtpExpiry"] = DateTime.Now.AddMinutes(5);

            user.OTP = otp;
            user.OTPCreatedAt = DateTime.Now;
            
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            _emailService.SendEmailAsync(email, "Your OTP is", otp);

            HttpContext.Session.SetString("UserName", Firstname);

            return RedirectToAction("VerifyOtp");
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string otp)
        {
            string storedOtp = TempData["OTP"]?.ToString();
            DateTime expiry = Convert.ToDateTime(TempData["OtpExpiry"]);

            if (storedOtp == null || DateTime.Now > expiry)
                return Content("OTP expired. Please request again.");

            if (otp == storedOtp)
            {
                // ✅ OTP verified — now sign in user
                string userName = HttpContext.Session.GetString("UserName");
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                var cookieOptions = new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
                HttpContext.Response.Cookies.Append("MyAppUser", "true", cookieOptions);
                HttpContext.Response.Cookies.Append("UserName", userName, cookieOptions);

                return RedirectToAction("Index", "Dashboard");
            }
            TempData.Keep(); // keep OTP alive for re-entry
            return Content("Invalid OTP. Try again.");
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        private string GenerateSecureOtp()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            int otp = BitConverter.ToInt32(bytes, 0) % 1000000;
            return Math.Abs(otp).ToString("D6");
        }
    }
}
