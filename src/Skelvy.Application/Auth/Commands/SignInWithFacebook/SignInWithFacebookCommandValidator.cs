using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandValidator : AbstractValidator<SignInWithFacebookCommand>
  {
    public SignInWithFacebookCommandValidator()
    {
      RuleFor(x => x.AuthToken).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(LanguageType.Check)
        .WithMessage(LanguageType.CheckFailedResponse());
    }
  }
}
