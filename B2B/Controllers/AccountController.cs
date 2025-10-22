using System.Security.Cryptography;
using B2B.Data;
using B2B.EmailService;
using B2B.Models;
using B2B.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace B2B.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
                return View(model);

            // Create and save user, company, bank info here
            //var user = new User
            //{
            //    FirstName = model.User.FirstName,
            //    LastName = model.User.LastName,
            //    Email = model.User.Email,
            //    PasswordHash = model.User.PasswordHash
            //};
            //_context.Users.Add(user);
            //_context.SaveChanges();

            // You can add company & bank saving logic here too
            return RedirectToAction("Index","Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> SendOtp(string clientEmail)
        {
            string otp = GenerateSecureOtp();
            await _emailService.SendEmailAsync(clientEmail, "Your OTP", otp);
            TempData["OTP"] = otp;
            TempData["OtpExpiry"] = DateTime.Now.AddMinutes(5);
            return Ok("OTP Sent Successfully");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
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

