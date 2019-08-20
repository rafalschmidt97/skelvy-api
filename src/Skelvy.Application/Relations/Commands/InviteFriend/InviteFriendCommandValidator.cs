using FluentValidation;

namespace Skelvy.Application.Relations.Commands.InviteFriend
{
  public class InviteFriendCommandValidator : AbstractValidator<InviteFriendCommand>
  {
    public InviteFriendCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.InvitingUserId).NotEmpty()
        .Unless(x => x.UserId != x.InvitingUserId)
        .WithMessage("'InvitingUserId' must be different than 'UserId'");
    }
  }
}
