using FluentValidation;
using Last02.Web.Models;

namespace Last02.Web.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("ユーザー名を入力してください。");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("パスワードを入力してください。");
        }
    }
}
