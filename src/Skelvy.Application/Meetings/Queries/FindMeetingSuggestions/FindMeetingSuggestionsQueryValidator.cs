using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeetingSuggestions
{
  public class FindMeetingSuggestionsQueryValidator : AbstractValidator<FindMeetingSuggestionsQuery>
  {
    public FindMeetingSuggestionsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Latitude).NotEmpty();
      RuleFor(x => x.Longitude).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(LanguageType.Check)
        .WithMessage(LanguageType.CheckFailedResponse());
    }
  }
}
