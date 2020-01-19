using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Maps.Queries.SearchLocations
{
  public class SearchLocationsQueryValidator : AbstractValidator<SearchLocationsQuery>
  {
    public SearchLocationsQueryValidator()
    {
      RuleFor(x => x.Search).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(LanguageType.Check)
        .WithMessage(LanguageType.CheckFailedResponse());
    }
  }
}
