namespace Last02.Web.Models
{
    public class UploadFlashcardViewModel
    {
        public int[] CourseIds { get; set; } = default!;
        public IFormFile FileContent { get; set; } = default!;
    }
}
