using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace B2B.ViewModels
{
    public class BusinessPaymentViewModel
    {
        [Display(Name = "Invoice ID")]
        public int InvoiceId { get; set; }

        [Display(Name = "Invoice Number")]
        public string? InvoiceNumber { get; set; }

        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        // --- Recipient (previously Beneficiary) ---
        [Required, Display(Name = "Recipient Name")]
        public string? RecipientName { get; set; }

        [Display(Name = "Recipient Email")]
        public string? RecipientEmail { get; set; }

        [Display(Name = "Recipient Country")]
        public string? RecipientCountry { get; set; }

        [Display(Name = "Recipient Address")]
        public string? RecipientAddress { get; set; }

        [Display(Name = "Bank Name")]
        public string? RecipientBankName { get; set; }

        [Display(Name = "Bank Country")]
        public string? RecipientBankCountry { get; set; }

        [Required, Display(Name = "Account / IBAN Number")]
        public string? RecipientAccountNumber { get; set; }

        [Display(Name = "SWIFT / BIC Code")]
        public string? RecipientSwiftCode { get; set; }

        // --- Payment Fields ---
        [Display(Name = "Currency")]
        public string Currency { get; set; } = "USD";

        [Required, Range(1, double.MaxValue)]
        [Display(Name = "Amount Due")]
        public decimal AmountDue { get; set; }

        [Display(Name = "Fee")]
        public decimal Fee { get; set; }

        [Display(Name = "FX Rate")]
        public decimal FxRate { get; set; } = 1.00m;

        [Display(Name = "Payment Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; } = DateTime.Now;

        [Display(Name = "Payment Description")]
        public string? PaymentDescription { get; set; }

        [Display(Name = "Attach Invoice")]
        public IFormFile? InvoiceFile { get; set; }

        // Calculated total
        public decimal Total => AmountDue + Fee;
    }
}
