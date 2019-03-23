using FluentValidation;
using Skelvy.Application.Users.Commands;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandValidator : AbstractValidator<SignInWithGoogleCommand>
  {
    public SignInWithGoogleCommandValidator()
    {
      RuleFor(x => x.AuthToken).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageTypes.EN || x == LanguageTypes.PL)
        .WithMessage($"'Language' must be {LanguageTypes.PL} or {LanguageTypes.EN}");
    }
  }
}
