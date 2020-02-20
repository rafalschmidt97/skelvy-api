using FluentValidation;

namespace Skelvy.Application.Groups.Commands.RemoveUserFromGroup
{
  public class RemoveUserFromGroupCommandValidator : AbstractValidator<RemoveUserFromGroupCommand>
  {
    public RemoveUserFromGroupCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();
      RuleFor(x => x.RemovingUserId).NotEmpty()
        .Unless(x => x.UserId != x.RemovingUserId)
        .WithMessage("'RemovingUserId' must be different than 'UserId'");
    }
  }
}
