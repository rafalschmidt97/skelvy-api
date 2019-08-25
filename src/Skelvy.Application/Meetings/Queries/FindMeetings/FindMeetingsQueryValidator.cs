using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeetings
{
  public class FindMeetingsQueryValidator : AbstractValidator<FindMeetingsQuery>
  {
    public FindMeetingsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
