using FluentValidation;

namespace Skelvy.Application.Users.Commands.RemoveBlockedUser
{
  public class RemoveBlockedUserCommandValidator : AbstractValidator<RemoveBlockedUserCommand>
  {
    public RemoveBlockedUserCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.BlockUserId).NotEmpty();

      RuleFor(x => x)
        .Must(x => x.BlockUserId != x.UserId)
        .WithMessage("'BlockUserId' must not be equal 'UserId");
    }
  }
}
