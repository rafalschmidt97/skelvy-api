using FluentValidation;

namespace Skelvy.Application.Users.Commands.DeleteUser
{
  public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
  {
    public DeleteUserCommandValidator()
    {
      RuleFor(x => x.Id).NotEmpty();
    }
  }
}
