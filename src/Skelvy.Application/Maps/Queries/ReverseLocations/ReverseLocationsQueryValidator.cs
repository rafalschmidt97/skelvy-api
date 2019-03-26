using FluentValidation;
using Skelvy.Application.Users.Commands;

namespace Skelvy.Application.Maps.Queries.ReverseLocations
{
  public class ReverseLocationsQueryValidator : AbstractValidator<ReverseLocationsQuery>
  {
    public ReverseLocationsQueryValidator()
    {
      RuleFor(x => x.Language).NotEmpty();
      RuleFor(x => x.Longitude).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageTypes.EN || x == LanguageTypes.PL)
        .WithMessage($"'Language' must be {LanguageTypes.PL} or {LanguageTypes.EN}");
    }
  }
}