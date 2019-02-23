using FluentValidation;

namespace Skelvy.Application.Meetings.Queries.GetMeeting
{
  public class GetMeetingQueryValidator : AbstractValidator<GetMeetingQuery>
  {
    public GetMeetingQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
