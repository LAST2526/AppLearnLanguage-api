using FluentValidation;
using Last02.Services.Interfaces;
using Last02.Web.Models;

namespace Last02.Web.Validators
{
    public class UploadFlashcardsValidator : AbstractValidator<UploadFlashcardViewModel>
    {
        private readonly ICourseService _courseService;
        public UploadFlashcardsValidator(ICourseService courseService)
        {
            _courseService = courseService;
            RuleFor(x => x.FileContent)
                .NotNull().WithMessage("Excelファイルが必要です")
                .Must(x => x.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                .WithMessage("Excelファイルが必要です")
                .Must(x => x.Length <= 10 * 1024 * 1024)
                .WithMessage("Excelファイルは10MB以下である必要があります");

            RuleFor(x => x.CourseIds)
                .NotEmpty().WithMessage("コースが必要です")
                .MustAsync(async (courseIds, cancellationToken) =>
                {
                    if (courseIds != null)
                    {
                        var courses = await _courseService.GetCoursesByIdsAsync(courseIds);
                        return courses.Count() == courseIds.Length;
                    }
                    else
                    {
                        return true;
                    }
                }).WithMessage("コースが必要です");
        }
    }
}
