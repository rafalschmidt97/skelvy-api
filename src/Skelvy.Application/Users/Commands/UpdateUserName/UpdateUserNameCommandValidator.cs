using System.Linq;
using FluentValidation;

namespace Skelvy.Application.Users.Commands.UpdateUserName
{
  public class UpdateUserNameCommandValidator : AbstractValidator<UpdateUserNameCommand>
  {
    public UpdateUserNameCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50)
        .Must(x => !x.All(char.IsWhiteSpace))
        .WithMessage("'Name' cannot contains whitespaces");
    }
  }
}
