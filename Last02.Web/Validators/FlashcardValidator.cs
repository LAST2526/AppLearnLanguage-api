using FluentValidation;
using Last02.Data.Entities;

namespace Last02.Web.Validators
{
    public class FlashcardValidator : AbstractValidator<Flashcard>
    {
        public FlashcardValidator()
        {
            RuleFor(x => x.Front)
                .NotEmpty().WithMessage("Front is required")
                .MaximumLength(500).WithMessage("Front must not exceed 500 characters");

            RuleFor(x => x.Furigana)
                .MaximumLength(200).WithMessage("Furigana must not exceed 200 characters");

            RuleFor(x => x.MeaningVi)
                .MaximumLength(1000).WithMessage("Meaning must not exceed 1000 characters");

            RuleFor(x => x.MeaningEn)
                .MaximumLength(1000).WithMessage("Meaning must not exceed 1000 characters");

            RuleFor(x => x.ExampleVi)
                .MaximumLength(2000).WithMessage("Example Vietnamese must not exceed 2000 characters");

            RuleFor(x => x.ExampleEn)
                .MaximumLength(2000).WithMessage("Example Indonesian must not exceed 2000 characters");

        }
    }
}
