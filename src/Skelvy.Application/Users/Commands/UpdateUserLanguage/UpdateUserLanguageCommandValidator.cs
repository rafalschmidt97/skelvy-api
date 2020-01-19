using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Users.Commands.UpdateUserLanguage
{
  public class UpdateUserLanguageCommandValidator : AbstractValidator<UpdateUserLanguageCommand>
  {
    public UpdateUserLanguageCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Language).NotEmpty()
        .Must(LanguageType.Check)
        .WithMessage(LanguageType.CheckFailedResponse());
    }
  }
}
