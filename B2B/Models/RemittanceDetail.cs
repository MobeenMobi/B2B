using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models
{
    public class RemittanceDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BankInfoId { get; set; }

        public virtual BankInfo BankInfo { get; set; }

        [Required]
        public string CountryName { get; set; }

        [Required]
        public string PurposeOfRemit { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
