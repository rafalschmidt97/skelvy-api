using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.LeaveGroup
{
  public class LeaveGroupCommandValidator : AbstractValidator<LeaveGroupCommand>
  {
    public LeaveGroupCommandValidator()
    {
      RuleFor(x => x.GroupId).NotEmpty();
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
