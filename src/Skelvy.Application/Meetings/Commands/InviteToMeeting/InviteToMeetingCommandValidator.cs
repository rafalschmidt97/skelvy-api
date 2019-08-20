using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.InviteToMeeting
{
  public class InviteToMeetingCommandValidator : AbstractValidator<InviteToMeetingCommand>
  {
    public InviteToMeetingCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.InvitedUserId).NotEmpty();
    }
  }
}
