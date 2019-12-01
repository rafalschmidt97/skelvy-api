using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQueryValidator : AbstractValidator<FindMeetingQuery>
  {
    public FindMeetingQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
