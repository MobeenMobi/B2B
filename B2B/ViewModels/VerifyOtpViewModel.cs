using System.ComponentModel.DataAnnotations;

namespace B2B.ViewModels
{
    public class VerifyOtpViewModel
    {
        [Required]
        [Display(Name = "OTP")]
        public string Otp { get; set; }
    }
}
