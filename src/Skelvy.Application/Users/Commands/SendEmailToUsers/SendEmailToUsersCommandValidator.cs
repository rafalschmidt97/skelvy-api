using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Users.Commands.SendEmailToUsers
{
  public class SendEmailToUsersCommandValidator : AbstractValidator<SendEmailToUsersCommand>
  {
    public SendEmailToUsersCommandValidator()
    {
      RuleFor(x => x.Subject).NotEmpty();
      RuleFor(x => x.Language).NotEmpty()
        .Must(LanguageType.Check)
        .WithMessage(LanguageType.CheckFailedResponse());
      RuleFor(x => x.Message).NotEmpty();
    }
  }
}
