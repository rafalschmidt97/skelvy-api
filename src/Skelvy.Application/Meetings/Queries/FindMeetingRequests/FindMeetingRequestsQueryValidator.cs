using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeetingRequests
{
  public class FindMeetingRequestsQueryValidator : AbstractValidator<FindMeetingRequestsQuery>
  {
    public FindMeetingRequestsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
