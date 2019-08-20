using FluentValidation;

namespace Skelvy.Application.Relations.Commands.RemoveBlocked
{
  public class RemoveBlockedCommandValidator : AbstractValidator<RemoveBlockedCommand>
  {
    public RemoveBlockedCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.BlockedUserId).NotEmpty()
        .Unless(x => x.UserId != x.BlockedUserId)
        .WithMessage("'BlockedUserId' must be different than 'UserId'");
    }
  }
}
