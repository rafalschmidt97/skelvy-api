using FluentValidation;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Application.Meetings.Commands.UpdateMeetingUserRole
{
  public class UpdateMeetingUserRoleCommandValidator : AbstractValidator<UpdateMeetingUserRoleCommand>
  {
    public UpdateMeetingUserRoleCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingId).NotEmpty();
      RuleFor(x => x.UpdatedUserId).NotEmpty();
      RuleFor(x => x.Role).NotEmpty().MaximumLength(15)
        .Must(x => x == GroupUserRoleType.Admin || x == GroupUserRoleType.Privileged || x == GroupUserRoleType.Member)
        .WithMessage($"'Role' must be {GroupUserRoleType.Admin} / {GroupUserRoleType.Privileged} / {GroupUserRoleType.Member}");
    }
  }
}
