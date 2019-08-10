using FluentValidation;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Maps.Queries.ReverseLocations
{
  public class ReverseLocationsQueryValidator : AbstractValidator<ReverseLocationsQuery>
  {
    public ReverseLocationsQueryValidator()
    {
      RuleFor(x => x.Longitude).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
