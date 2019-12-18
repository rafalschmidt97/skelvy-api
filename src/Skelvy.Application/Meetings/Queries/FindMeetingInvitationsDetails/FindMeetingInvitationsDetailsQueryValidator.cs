using FluentValidation;

namespace Skelvy.Application.Meetings.Queries.FindMeetingInvitationsDetails
{
  public class FindMeetingInvitationsDetailsQueryValidator : AbstractValidator<FindMeetingInvitationsDetailsQuery>
  {
    public FindMeetingInvitationsDetailsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingId).NotEmpty();
    }
  }
}
