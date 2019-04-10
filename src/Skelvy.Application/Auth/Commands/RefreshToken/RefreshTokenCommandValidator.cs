using FluentValidation;

namespace Skelvy.Application.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
  {
    public RefreshTokenCommandValidator()
    {
      RuleFor(x => x.RefreshToken).NotEmpty();
    }
  }
}
