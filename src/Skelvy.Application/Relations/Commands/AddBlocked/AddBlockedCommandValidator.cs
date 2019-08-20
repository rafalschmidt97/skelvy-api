using FluentValidation;

namespace Skelvy.Application.Relations.Commands.AddBlocked
{
  public class AddBlockedCommandValidator : AbstractValidator<AddBlockedCommand>
  {
    public AddBlockedCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.RelatedUserId).NotEmpty();
    }
  }
}
