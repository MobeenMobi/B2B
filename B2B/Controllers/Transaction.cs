using B2B.Data;
using B2B.Models;
using B2B.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace B2B.Controllers
{
    public class Transaction : Controller
    {
        private readonly ApplicationDbContext _context;
        public Transaction(ApplicationDbContext context)
        {
              _context = context;  
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SinglePayment()
        {
            BusinessPaymentViewModel model = new BusinessPaymentViewModel();

            return View(model);
        }
        [HttpPost]
        public IActionResult SinglePayment(BusinessPaymentViewModel model)
        {
            var payeeName = HttpContext.Session.GetString("UserName") ?? "Unknown";

            // Handle file upload (optional)
            string filePath = null;
            if (model.InvoiceFile != null && model.InvoiceFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/invoices");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.InvoiceFile.FileName);
                filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.InvoiceFile.CopyTo(stream);
                }

                filePath = "/uploads/invoices/" + fileName;
            }

            var entity = new Payments
            {
                PayeeName = payeeName,
                RecipientName = model.RecipientName,
                RecipientEmail = model.RecipientEmail,
                RecipientCountry = model.RecipientCountry,
                RecipientAddress = model.RecipientAddress,
                RecipientBankName = model.RecipientBankName,
                RecipientBankCountry = model.RecipientBankCountry,
                RecipientAccountNumber = model.RecipientAccountNumber,
                RecipientSwiftCode = model.RecipientSwiftCode,
                Currency = model.Currency,
                AmountDue = model.AmountDue,
                Fee = model.Fee,
                FxRate = model.FxRate,
                DueDate = model.DueDate,
                PaymentDescription = model.PaymentDescription,
                InvoiceFilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            // Save to DB
            _context.Payments.Add(entity);
            _context.SaveChanges();

            TempData["Success"] = "Payment successfully recorded.";
            return RedirectToAction("SinglePayment");
        }

    }
}
