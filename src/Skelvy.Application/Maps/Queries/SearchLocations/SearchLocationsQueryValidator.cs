using FluentValidation;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Maps.Queries.SearchLocations
{
  public class SearchLocationsQueryValidator : AbstractValidator<SearchLocationsQuery>
  {
    public SearchLocationsQueryValidator()
    {
      RuleFor(x => x.Search).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
