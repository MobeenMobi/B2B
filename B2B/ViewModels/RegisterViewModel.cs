using B2B.Models;

namespace B2B.ViewModels
{
    public class RegisterViewModel
    {
        public CompanyInfo Company { get; set; }
        public BankInfoViewModel Bank { get; set; }
        public UserViewModel User { get; set; }
        public DocumentUploadViewModel DocumentUpload { get; set; }

    }
}
