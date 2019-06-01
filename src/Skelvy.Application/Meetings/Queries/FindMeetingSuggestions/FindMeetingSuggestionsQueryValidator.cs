using FluentValidation;

namespace Skelvy.Application.Meetings.Queries.FindMeetingSuggestions
{
  public class FindMeetingSuggestionsQueryValidator : AbstractValidator<FindMeetingSuggestionsQuery>
  {
    public FindMeetingSuggestionsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Latitude).NotEmpty();
      RuleFor(x => x.Longitude).NotEmpty();
    }
  }
}
