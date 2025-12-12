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
                    AccountNumber = model.Bank.AccountNumber,
                    AverageAnnualTrunover = model.Bank.AverageAnnualTrunover
                    
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
                            UploadDate = DateTime.Now
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
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    LastLogin = null,
                    OTP = "",
                    OTPCreatedAt = null,
                    RoleId = 6
                };

                _context.UserLogins.Add(userLogin);
                _context.SaveChanges();


                TempData["Message"] = "Registration successful! Your KYB is pending. Once it's done you will be able to login.";
                return RedirectToAction("Index", "Login");

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult EditRegistration(int id)
        {
            // Load User
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            // Load Company Info
            var company = _context.CompanyInfo.FirstOrDefault(c => c.UserId == id);

            // Load Bank Info
            var bank = _context.BankInfo.FirstOrDefault(b => b.UserId == id);

            // Load Remittance List
            var remittance = _context.RemittanceDetail
                .Where(r => r.BankInfoId == bank.Id)
                .ToList();

            // Load Documents
            var documents = _context.Documents
                .Where(d => d.UserId == id)
                .ToList();

            // Build ViewModel
            var model = new RegisterViewModel
            {
                User = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    IDType = user.IDType,
                    IDNumber = user.IDNumber,
                    PhoneNumber = user.PhoneNumber,
                    Occupation = user.Occupation,
                    ApproverName = user.ApproverName,
                    Agree = user.Agree,
                    Email = _context.UserLogins.FirstOrDefault(x => x.UserId == id)?.Email,
                },

                Company = new CompanyInfo
                {
                    CompanyName = company?.CompanyName,
                    RegistrationNumber = company?.RegistrationNumber,
                    DateOfIncorporation = company.DateOfIncorporation,
                    BusinessType = company?.BusinessType,
                    NatureOfBusiness = company?.NatureOfBusiness,
                    ShortBusinessDescription = company?.ShortBusinessDescription,
                    CompanyEmail = company?.CompanyEmail,
                    CompanyWebsite = company?.CompanyWebsite,
                    PhoneNo = company?.PhoneNo,
                    AddressLine1 = company?.AddressLine1,
                    AddressLine2 = company?.AddressLine2,
                    AddressLine3 = company?.AddressLine3,
                    City = company?.City,
                    Postcode = company?.Postcode,
                    State = company?.State,
                    Country = company?.Country,
                },

                Bank = new BankInfoViewModel
                {
                    Bank = bank?.Bank,
                    AccountHolder = bank?.AccountHolder,
                    AccountNumber = bank?.AccountNumber,
                    AverageAnnualTrunover = bank.AverageAnnualTrunover,
                    RemittanceDetails = remittance.Select(r => new RemittanceDetailViewModel
                    {
                        CountryName = r.CountryName,
                        PurposeOfRemit = r.PurposeOfRemit
                    }).ToList()
                }
            };

            ViewBag.IsEdit = true; // to know if saving should call Edit instead of Register

            return View("Register", model);  // Reuse same view
        }

        [HttpPost]
        public IActionResult EditRegistration(int id, RegisterViewModel model)
        {
            // Load DB objects
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            var company = _context.CompanyInfo.FirstOrDefault(c => c.UserId == id);
            var bank = _context.BankInfo.FirstOrDefault(b => b.UserId == id);
            var login = _context.UserLogins.FirstOrDefault(l => l.UserId == id);

            if (user == null)
                return NotFound();

            // Update User
            user.FirstName = model.User.FirstName;
            user.MiddleName = model.User.MiddleName;
            user.LastName = model.User.LastName;
            user.IDType = model.User.IDType;
            user.IDNumber = model.User.IDNumber;
            user.PhoneNumber = model.User.PhoneNumber;
            user.Occupation = model.User.Occupation;
            user.ApproverName = model.User.ApproverName;
            user.Agree = model.User.Agree;

            _context.Users.Update(user);

            // Update Company Info
            if (company != null)
            {
                company.CompanyName = model.Company.CompanyName;
                company.RegistrationNumber = model.Company.RegistrationNumber;
                company.DateOfIncorporation = model.Company.DateOfIncorporation;
                company.BusinessType = model.Company.BusinessType;
                company.NatureOfBusiness = model.Company.NatureOfBusiness;
                company.ShortBusinessDescription = model.Company.ShortBusinessDescription;
                company.CompanyEmail = model.Company.CompanyEmail;
                company.CompanyWebsite = model.Company.CompanyWebsite;
                company.PhoneNo = model.Company.PhoneNo;
                company.AddressLine1 = model.Company.AddressLine1;
                company.AddressLine2 = model.Company.AddressLine2;
                company.AddressLine3 = model.Company.AddressLine3;
                company.City = model.Company.City;
                company.Postcode = model.Company.Postcode;
                company.State = model.Company.State;
                company.Country = model.Company.Country;

                _context.CompanyInfo.Update(company);
            }

            // Update Bank Info
            if (bank != null)
            {
                bank.Bank = model.Bank.Bank;
                bank.AccountHolder = model.Bank.AccountHolder;
                bank.AccountNumber = model.Bank.AccountNumber;
                bank.AverageAnnualTrunover = model.Bank.AverageAnnualTrunover;

                _context.BankInfo.Update(bank);
            }

            // Update Remittance Details
            var oldRemittances = _context.RemittanceDetail.Where(r => r.BankInfoId == bank.Id);
            _context.RemittanceDetail.RemoveRange(oldRemittances);

            if (model.Bank.RemittanceDetails != null)
            {
                foreach (var item in model.Bank.RemittanceDetails)
                {
                    _context.RemittanceDetail.Add(new RemittanceDetail
                    {
                        BankInfoId = bank.Id,
                        CountryName = item.CountryName,
                        PurposeOfRemit = item.PurposeOfRemit,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            // Update Login
            if (login != null)
            {
                login.Email = model.User.Email;
                login.PasswordHash = model.User.Password; // hash later
                _context.UserLogins.Update(login);
            }

            // Upload new documents
            if (model.DocumentUpload != null)
            {
                if (model.DocumentUpload.Documents != null)
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    foreach (var file in model.DocumentUpload.Documents)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(uploadPath, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        var document = new Documents
                        {
                            UserId = id,
                            DocumentName = file.FileName,
                            DocumentPath = "/wwwroot/uploads/" + uniqueFileName,
                            DocumentType = file.ContentType,
                            UploadDate = DateTime.Now
                        };

                        _context.Documents.Add(document);
                    }
                }
            }
            _context.SaveChanges();


            TempData["Message"] = "Registration update successful! Your KYB is pending. Once it's done you will be able to login.";
            return RedirectToAction("Index", "Login");
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

