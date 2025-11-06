using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models
{
    public class Payments
    {
        [Key]
        public int PaymentId { get; set; }

        [Required, MaxLength(150)]
        public string RecipientName { get; set; }

        [MaxLength(150)]
        public string RecipientEmail { get; set; }

        [MaxLength(100)]
        public string RecipientCountry { get; set; }

        [MaxLength(250)]
        public string RecipientAddress { get; set; }

        [MaxLength(150)]
        public string RecipientBankName { get; set; }

        [MaxLength(100)]
        public string RecipientBankCountry { get; set; }

        [MaxLength(100)]
        public string RecipientAccountNumber { get; set; }

        [MaxLength(50)]
        public string RecipientSwiftCode { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountDue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Fee { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? FxRate { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(500)]
        public string PaymentDescription { get; set; }

        [MaxLength(5000)]
        public string InvoiceFilePath { get; set; } // store file path, not the actual file

        [MaxLength(150)]
        public string PayeeName { get; set; } // From logged-in user

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Status { get; set; }
    }
}
