using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.LeaveMeeting
{
  public class LeaveMeetingCommandValidator : AbstractValidator<LeaveMeetingCommand>
  {
    public LeaveMeetingCommandValidator()
    {
      RuleFor(x => x.MeetingId).NotEmpty();
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
