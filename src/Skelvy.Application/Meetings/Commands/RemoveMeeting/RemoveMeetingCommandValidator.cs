using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.RemoveMeeting
{
  public class RemoveMeetingCommandValidator : AbstractValidator<RemoveMeetingCommand>
  {
    public RemoveMeetingCommandValidator()
    {
      RuleFor(x => x.MeetingId).NotEmpty();
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
