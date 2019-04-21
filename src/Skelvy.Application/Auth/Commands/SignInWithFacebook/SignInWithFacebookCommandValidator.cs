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
        .Must(x => x == LanguageTypes.EN || x == LanguageTypes.PL)
        .WithMessage($"'Language' must be {LanguageTypes.PL} or {LanguageTypes.EN}");
    }
  }
}
