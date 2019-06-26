using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.ConnectMeetingRequest
{
  public class ConnectMeetingRequestCommandValidator : AbstractValidator<ConnectMeetingRequestCommand>
  {
    public ConnectMeetingRequestCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingRequestId).NotEmpty();
    }
  }
}
