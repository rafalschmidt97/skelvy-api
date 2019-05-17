using FluentValidation;

namespace Skelvy.Application.Users.Commands.AddBlockedUser
{
  public class AddBlockedUserCommandValidator : AbstractValidator<AddBlockedUserCommand>
  {
    public AddBlockedUserCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.BlockUserId).NotEmpty();

      RuleFor(x => x)
        .Must(x => x.BlockUserId != x.UserId)
        .WithMessage("'BlockUserId' must not be equal 'UserId");
    }
  }
}
