using FluentValidation;

namespace Skelvy.Application.Auth.Commands.Logout
{
  public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
  {
    public LogoutCommandValidator()
    {
      RuleFor(x => x.RefreshToken).NotEmpty();
    }
  }
}
