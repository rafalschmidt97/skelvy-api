using FluentValidation;

namespace Skelvy.Application.Relations.Commands.RemoveFriend
{
  public class RemoveFriendCommandValidator : AbstractValidator<RemoveFriendCommand>
  {
    public RemoveFriendCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.FriendUserId).NotEmpty()
        .Unless(x => x.UserId != x.FriendUserId)
        .WithMessage("'FriendUserId' must be different than 'UserId'");
    }
  }
}
