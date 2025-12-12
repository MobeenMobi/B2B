using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models
{
    public class CompanyInfo
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime DateOfIncorporation { get; set; }
        public string BusinessType { get; set; }
        public string NatureOfBusiness { get; set; }
        public string ShortBusinessDescription { get; set; }
        public string CompanyEmail { get; set; }
        public string? CompanyWebsite { get; set; }
        public string PhoneNo { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string Country { get; set; } = "Malaysia";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? LocationOfIncorporation { get; set; }
    }
}
