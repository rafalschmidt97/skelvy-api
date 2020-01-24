using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Users.Commands.SendEmailToUser
{
  public class SendEmailToUserCommandValidator : AbstractValidator<SendEmailToUserCommand>
  {
    public SendEmailToUserCommandValidator()
    {
      RuleFor(x => x.To).NotEmpty();
      RuleFor(x => x.Subject).NotEmpty();
      RuleFor(x => x.Language).NotEmpty()
        .Must(LanguageType.Check)
        .WithMessage(LanguageType.CheckFailedResponse());
      RuleFor(x => x.Message).NotEmpty();
    }
  }
}
