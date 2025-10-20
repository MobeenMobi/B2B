namespace B2B.Models
{
    public class UserInfo
    {
        public string ControlOption { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
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
        public string Password { get; set; }

        public string RePassword { get; set; }

        public List<IFormFile> Documents { get; set; }

        public bool Agree { get; set; }
    }
}
