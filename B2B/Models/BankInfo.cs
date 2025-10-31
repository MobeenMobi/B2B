using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models
{
    public class BankInfo
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Bank { get; set; }
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
