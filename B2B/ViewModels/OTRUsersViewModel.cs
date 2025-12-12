namespace B2B.ViewModels
{
    public class OTRUsersViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Occupation { get; set; }
        public string? ApproverName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}

