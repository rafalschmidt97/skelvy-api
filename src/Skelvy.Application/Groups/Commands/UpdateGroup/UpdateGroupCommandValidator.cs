using FluentValidation;

namespace Skelvy.Application.Groups.Commands.UpdateGroup
{
  public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
  {
    public UpdateGroupCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();
      RuleFor(x => x.Name).MinimumLength(3).MaximumLength(50);
    }
  }
}
