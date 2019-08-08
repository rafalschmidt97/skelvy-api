using FluentValidation;

namespace Skelvy.Application.Relations.Commands.InviteFriend
{
  public class InviteFriendCommandValidator : AbstractValidator<InviteFriendCommand>
  {
    public InviteFriendCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.InvitedUserId).NotEmpty();
    }
  }
}
