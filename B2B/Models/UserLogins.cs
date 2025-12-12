using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models
{
    public class UserLogins
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Role { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPCreatedAt { get; set; }
    }
}
