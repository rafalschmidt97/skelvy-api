using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.RemoveUserFromMeeting
{
  public class RemoveUserFromMeetingCommandValidator : AbstractValidator<RemoveUserFromMeetingCommand>
  {
    public RemoveUserFromMeetingCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingId).NotEmpty();
      RuleFor(x => x.RemovedUserId).NotEmpty();
    }
  }
}
