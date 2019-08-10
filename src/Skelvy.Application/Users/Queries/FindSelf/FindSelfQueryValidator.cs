using FluentValidation;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Users.Queries.FindSelf
{
  public class FindSelfQueryValidator : AbstractValidator<FindSelfQuery>
  {
    public FindSelfQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
