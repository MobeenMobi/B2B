using B2B.Data;
using B2B.EmailService;
using B2B.Models;
using B2B.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            try
            {
                var user = new Users
                {
                    FirstName = model.User.FirstName,
                    MiddleName = model.User.MiddleName,
                    LastName = model.User.LastName,
                    IDType = model.User.IDType,
                    IDNumber = model.User.IDNumber,
                    PhoneNumber = model.User.PhoneNumber,
                    Occupation = model.User.Occupation,
                    ApproverName = model.User.ApproverName,
                    Agree = model.User.Agree

                };

                _context.Users.Add(user);
                _context.SaveChanges();

                var latestUserId = _context.Users
                .OrderByDescending(u => u.Id)
                .Select(u => u.Id)
                .FirstOrDefault();

                var company = new CompanyInfo
                {
                    UserId = latestUserId,
                    CompanyName = model.Company.CompanyName,
                    RegistrationNumber = model.Company.RegistrationNumber,
                    DateOfIncorporation = model.Company.DateOfIncorporation,
                    BusinessType = model.Company.BusinessType,
                    NatureOfBusiness = model.Company.NatureOfBusiness,
                    ShortBusinessDescription = model.Company.ShortBusinessDescription,
                    CompanyEmail = model.Company.CompanyEmail,
                    CompanyWebsite = model.Company.CompanyWebsite,
                    PhoneNo = model.Company.PhoneNo,
                    AddressLine1 = model.Company.AddressLine1,
                    AddressLine2 = model.Company.AddressLine2,
                    AddressLine3 = model.Company.AddressLine3,
                    City = model.Company.City,
                    Postcode = model.Company.Postcode,
                    State = model.Company.State,
                    Country = model.Company.Country
                };

                _context.CompanyInfo.Add(company);
                _context.SaveChanges();

                var bank = new BankInfo
                {
                    UserId = latestUserId,
                    Bank = model.Bank.Bank,
                    AccountHolder = model.Bank.AccountHolder,
                    AccountNumber = model.Bank.AccountNumber
                };

                _context.BankInfo.Add(bank);
                _context.SaveChanges();

                var latestBankId = _context.BankInfo
                    .OrderByDescending(b => b.Id)
                    .Select(b => b.Id)
                    .FirstOrDefault();

                if (model.Bank.RemittanceDetails != null)
                {
                    foreach (var item in model.Bank.RemittanceDetails)
                    {
                        var RemittanceDetails = new RemittanceDetail
                        {
                            BankInfoId = latestBankId,
                            CountryName = item.CountryName,
                            PurposeOfRemit = item.PurposeOfRemit,
                            CreatedAt = DateTime.Now
                        };
                        _context.RemittanceDetail.Add(RemittanceDetails);
                        _context.SaveChanges();
                    }
                }

                if (model.DocumentUpload.Documents != null)
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    foreach (var file in model.DocumentUpload.Documents)
                    {
                        // 1) SAVE FILE PHYSICALLY
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(uploadPath, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        // 2) SAVE IN DATABASE
                        var document = new Documents
                        {
                            UserId = latestUserId,
                            DocumentName = file.FileName,
                            DocumentPath = "/wwwroot/uploads/" + uniqueFileName,
                            DocumentType = file.ContentType,
                            UploadDate = DateTime.UtcNow
                        };

                        _context.Documents.Add(document);
                        _context.SaveChanges();
                    }
                }

                var userLogin = new UserLogins
                {
                    UserId = latestUserId,
                    Email = model.User.Email,
                    PasswordHash = model.User.Password,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.UserLogins.Add(userLogin);
                _context.SaveChanges();


                ViewBag.Message = "Registration successful! Your KYB is pending once its done you will be able to lgoin.";
                return RedirectToAction("Index", "Login");

            }
            catch (Exception)
            {

                throw;
            }
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

