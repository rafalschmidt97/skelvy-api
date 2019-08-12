using FluentValidation;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Meetings.Queries.FindGroups
{
  public class FindGroupsQueryValidator : AbstractValidator<FindGroupsQuery>
  {
    public FindGroupsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
