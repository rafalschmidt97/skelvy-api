using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.InviteToMeeting
{
  public class InviteToMeetingCommandValidator : AbstractValidator<InviteToMeetingCommand>
  {
    public InviteToMeetingCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.InvitingUserId).NotEmpty()
        .Unless(x => x.UserId != x.InvitingUserId)
        .WithMessage("'InvitingUserId' must be different than 'UserId'");
    }
  }
}
