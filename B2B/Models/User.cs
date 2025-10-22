using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace B2B.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string ControlOption { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string PhonePrefix { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string Occupation { get; set; }
        public string ApproverName { get; set; }
        public bool Agree { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual UserLogins Login { get; set; }
        public virtual ICollection<Documents> Documents { get; set; } = new List<Documents>();
        public virtual ICollection<CompanyInfo> Companies { get; set; } = new List<CompanyInfo>();
        public virtual ICollection<BankInfo> BankAccounts { get; set; } = new List<BankInfo>();
    }
}
