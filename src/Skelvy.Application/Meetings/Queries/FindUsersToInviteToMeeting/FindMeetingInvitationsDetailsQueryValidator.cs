using FluentValidation;

namespace Skelvy.Application.Meetings.Queries.FindUsersToInviteToMeeting
{
  public class FindMeetingInvitationsDetailsQueryValidator : AbstractValidator<FindUsersToInviteToMeetingQuery>
  {
    public FindMeetingInvitationsDetailsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingId).NotEmpty();
      RuleFor(x => x.Page).NotEmpty();
    }
  }
}
