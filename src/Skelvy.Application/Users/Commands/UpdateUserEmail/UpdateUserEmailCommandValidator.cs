using FluentValidation;

namespace Skelvy.Application.Users.Commands.UpdateUserEmail
{
  public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
  {
    public UpdateUserEmailCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
  }
}
