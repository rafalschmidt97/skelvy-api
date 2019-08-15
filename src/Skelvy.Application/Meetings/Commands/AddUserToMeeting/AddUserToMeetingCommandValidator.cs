using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.AddUserToMeeting
{
  public class AddUserToMeetingCommandValidator : AbstractValidator<AddUserToMeetingCommand>
  {
    public AddUserToMeetingCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingId).NotEmpty();
      RuleFor(x => x.AddedUserId).NotEmpty();
    }
  }
}
