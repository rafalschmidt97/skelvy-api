using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.JoinMeeting
{
  public class JoinMeetingCommandValidator : AbstractValidator<JoinMeetingCommand>
  {
    public JoinMeetingCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingId).NotEmpty();
    }
  }
}
