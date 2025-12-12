namespace B2B.Models
{
    public class Roles
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
