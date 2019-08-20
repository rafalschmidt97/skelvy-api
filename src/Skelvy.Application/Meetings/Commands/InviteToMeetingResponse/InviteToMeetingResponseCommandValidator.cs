using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.InviteToMeetingResponse
{
  public class InviteToMeetingResponseCommandValidator : AbstractValidator<InviteToMeetingResponseCommand>
  {
    public InviteToMeetingResponseCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.InvitationId).NotEmpty();
    }
  }
}
