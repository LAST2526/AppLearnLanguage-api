namespace Last02.Web.Models
{
    public class UploadCsvAudioViewModel
    {
        public int[] CourseIds { get; set; } = [];
        public IFormFile FileContent { get; set; } = default!;
    }
}
