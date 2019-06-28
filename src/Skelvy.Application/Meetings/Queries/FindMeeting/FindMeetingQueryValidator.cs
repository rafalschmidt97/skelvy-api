using FluentValidation;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQueryValidator : AbstractValidator<FindMeetingQuery>
  {
    public FindMeetingQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageTypes.EN || x == LanguageTypes.PL)
        .WithMessage($"'Language' must be {LanguageTypes.PL} or {LanguageTypes.EN}");
    }
  }
}
