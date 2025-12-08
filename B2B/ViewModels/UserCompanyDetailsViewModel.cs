namespace B2B.ViewModels
{
    public class UserCompanyDetailsViewModel
    {
        // User fields
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ApproverName { get; set; }
        public bool IsKYBApproved { get; set; } = false;

        // CompanyInfo fields
        public string? CompanyName { get; set; }
        public string? BusinessType { get; set; }
    }
}
