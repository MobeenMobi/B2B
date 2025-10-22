namespace B2B.ViewModels
{
    public class DocumentUploadViewModel
    {
        public int UserId { get; set; }
        public List<IFormFile> Documents { get; set; } 
    }
}
