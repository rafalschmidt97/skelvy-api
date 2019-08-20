using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandValidator : AbstractValidator<SignInWithGoogleCommand>
  {
    public SignInWithGoogleCommandValidator()
    {
      RuleFor(x => x.AuthToken).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
