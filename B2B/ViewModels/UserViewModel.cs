using System.ComponentModel.DataAnnotations;

namespace B2B.ViewModels
{
    public class UserViewModel
    {
        public int? Id { get; set; }
        public string ControlOption { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string Occupation { get; set; }
        public string ApproverName { get; set; }
        public bool Agree { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsKYBApproved { get; set; } = false;
    }
}
