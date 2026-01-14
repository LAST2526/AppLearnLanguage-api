using FluentValidation;
using Last02.Services.Interfaces;
using Last02.Web.Models;

namespace Last02.Web.Validators
{
    public class UploadAudiosValidator : AbstractValidator<UploadCsvAudioViewModel>
    {
        private readonly ICourseService _courseService;
        public UploadAudiosValidator(ICourseService courseService)
        {
            _courseService = courseService;
            RuleFor(x => x.FileContent)
                .NotEmpty()
                .WithMessage(Resources.Resource.Common_Error_File_Required);
            RuleFor(x => x.FileContent)
                .Must(x => x.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                .WithMessage("Excelƒtƒ@ƒCƒ‹‚ª•K—v‚Å‚·");
            RuleFor(x => x.FileContent)
                .Must(x => x.Length < 10 * 1024 * 1024)
                .WithMessage(Resources.Resource.Common_Error_File_Maxsize);
            RuleFor(x => x.CourseIds)
                .NotEmpty()
                .WithMessage(Resources.Resource.Common_Error_Course_Required);
            RuleFor(x => x.CourseIds)
                .Must(x =>
                {
                    return _courseService.GetCoursesByIdsAsync(x).Result.Count() == x.Length;
                })
                .WithMessage(Resources.Resource.Common_Error_Course_Invalid);
        }
    }
}
