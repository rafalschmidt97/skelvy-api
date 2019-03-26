using FluentValidation;
using Skelvy.Application.Users.Commands;

namespace Skelvy.Application.Maps.Queries.SearchLocations
{
  public class SearchLocationsQueryValidator : AbstractValidator<SearchLocationsQuery>
  {
    public SearchLocationsQueryValidator()
    {
      RuleFor(x => x.Search).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageTypes.EN || x == LanguageTypes.PL)
        .WithMessage($"'Language' must be {LanguageTypes.PL} or {LanguageTypes.EN}");
    }
  }
}
