using FluentValidation;
using Last02.Data.Entities;

namespace Last02.Web.Validators
{
    public class AudioValidator : AbstractValidator<Audio>
    {
        public AudioValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(Resources.Resource.Common_Error_Title_Required);
            RuleFor(x => x.AudioCode)
                .NotEmpty()
                .WithMessage(Resources.Resource.Common_Error_AudioCode_Required);
            RuleFor(x => x.FileUrl)
                .Must(url =>
                {
                    if (string.IsNullOrWhiteSpace(url))
                        return true;

                    if (url.StartsWith("/"))
                        return true;

                    return Uri.TryCreate(url, UriKind.Absolute, out _);
                })
                .WithMessage(Resources.Resource.Common_Error_Url_Invalid);
        }
    }
}
