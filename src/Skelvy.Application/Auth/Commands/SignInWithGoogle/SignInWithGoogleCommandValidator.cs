using FluentValidation;

namespace Skelvy.Application.Auth.Commands.SignInWithGoogle
{
  public class SignInWithGoogleCommandValidator : AbstractValidator<SignInWithGoogleCommand>
  {
    public SignInWithGoogleCommandValidator()
    {
      RuleFor(x => x.AuthToken).NotEmpty();
    }
  }
}
