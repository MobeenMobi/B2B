using System.ComponentModel.DataAnnotations;

namespace B2B.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string ControlOption { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public string? ApproverName { get; set; }
        public bool Agree { get; set; }
        public bool IsKYBApproved { get; set; } = false;
    }
}
