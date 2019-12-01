using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Groups.Queries.FindGroup
{
  public class FindGroupQueryValidator : AbstractValidator<FindGroupQuery>
  {
    public FindGroupQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
