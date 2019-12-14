using FluentValidation;

namespace Skelvy.Application.Relations.Commands.InviteFriendResponse
{
  public class InviteFriendResponseCommandValidator : AbstractValidator<InviteFriendResponseCommand>
  {
    public InviteFriendResponseCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.InvitationId).NotEmpty();
    }
  }
}
