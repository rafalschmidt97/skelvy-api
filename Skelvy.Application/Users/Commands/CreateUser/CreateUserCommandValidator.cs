using FluentValidation;

namespace Skelvy.Application.Users.Commands.CreateUser
{
  public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
  {
    public CreateUserCommandValidator()
    {
      RuleFor(x => x.Email).MaximumLength(60).NotEmpty();
      RuleFor(x => x.Name).MaximumLength(60).NotEmpty();
    }
  }
}
