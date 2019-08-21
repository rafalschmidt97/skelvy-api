using FluentValidation;

namespace Skelvy.Application.Relations.Queries.FindFriendInvitations
{
  public class FindFriendInvitationsQueryValidator : AbstractValidator<FindFriendInvitationsQuery>
  {
    public FindFriendInvitationsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
