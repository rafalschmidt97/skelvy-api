using FluentValidation;

namespace Skelvy.Application.Users.Commands.DisableUser
{
  public class DisableUserCommandValidator : AbstractValidator<DisableUserCommand>
  {
    public DisableUserCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Reason).NotEmpty();
    }
  }
}
