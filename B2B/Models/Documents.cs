using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models
{
    public class Documents
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string DocumentType { get; set; }

    }
}
