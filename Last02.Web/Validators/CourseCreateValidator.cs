using FluentValidation;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;

namespace Last02.Web.Validators
{
    public class CourseCreateValidator : AbstractValidator<CourseCreateDto>
    {
        private readonly ICourseService _courseService;
        public CourseCreateValidator(ICourseService courseService)
        {
            _courseService = courseService;

            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must be 200 characters or less.")
                .Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage("Title cannot consist of only whitespace.");
        }
    }
}
