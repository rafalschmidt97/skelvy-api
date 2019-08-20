using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Users.Queries.Sync
{
  public class SyncQueryValidator : AbstractValidator<SyncQuery>
  {
    public SyncQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(x => x == LanguageType.EN || x == LanguageType.PL)
        .WithMessage($"'Language' must be {LanguageType.PL} or {LanguageType.EN}");
    }
  }
}
