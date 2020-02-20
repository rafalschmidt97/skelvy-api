using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Groups.Commands.UpdateGroupUserRole
{
  public class UpdateGroupUserRoleCommandValidator : AbstractValidator<UpdateGroupUserRoleCommand>
  {
    public UpdateGroupUserRoleCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();
      RuleFor(x => x.UpdatedUserId).NotEmpty();
      RuleFor(x => x.Role).NotEmpty().MaximumLength(15)
        .Must(x => x == GroupUserRoleType.Admin || x == GroupUserRoleType.Privileged || x == GroupUserRoleType.Member)
        .WithMessage($"'Role' must be {GroupUserRoleType.Admin} / {GroupUserRoleType.Privileged} / {GroupUserRoleType.Member}");
    }
  }
}
