using FluentValidation;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommandValidator : AbstractValidator<RemoveUserCommand>
  {
    public RemoveUserCommandValidator()
    {
      RuleFor(x => x.Id).NotEmpty();
    }
  }
}
