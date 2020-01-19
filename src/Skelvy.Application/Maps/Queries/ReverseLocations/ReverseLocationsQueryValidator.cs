using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Maps.Queries.ReverseLocations
{
  public class ReverseLocationsQueryValidator : AbstractValidator<ReverseLocationsQuery>
  {
    public ReverseLocationsQueryValidator()
    {
      RuleFor(x => x.Longitude).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(LanguageType.Check)
        .WithMessage(LanguageType.CheckFailedResponse());
    }
  }
}
