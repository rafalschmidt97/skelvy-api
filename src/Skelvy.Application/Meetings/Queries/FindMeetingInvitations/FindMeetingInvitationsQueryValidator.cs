using FluentValidation;

namespace Skelvy.Application.Meetings.Queries.FindMeetingInvitations
{
  public class FindMeetingInvitationsQueryValidator : AbstractValidator<FindMeetingInvitationsQuery>
  {
    public FindMeetingInvitationsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
