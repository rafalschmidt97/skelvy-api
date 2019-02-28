using FluentValidation;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQueryValidator : AbstractValidator<FindMeetingQuery>
  {
    public FindMeetingQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
