using FluentValidation;

namespace Skelvy.Application.Auth.Commands.SignInWithFacebook
{
  public class SignInWithFacebookCommandValidator : AbstractValidator<SignInWithFacebookCommand>
  {
    public SignInWithFacebookCommandValidator()
    {
      RuleFor(x => x.AuthToken).NotEmpty();
    }
  }
}
