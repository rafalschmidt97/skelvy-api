using FluentValidation;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandValidator : AbstractValidator<SignInWithFacebookCommand>
  {
    public SignInWithFacebookCommandValidator()
    {
      RuleFor(x => x.AuthToken).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
