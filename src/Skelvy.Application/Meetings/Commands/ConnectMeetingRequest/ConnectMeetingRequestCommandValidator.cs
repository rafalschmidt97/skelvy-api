using System;
using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.ConnectMeetingRequest
{
  public class ConnectMeetingRequestCommandValidator : AbstractValidator<ConnectMeetingRequestCommand>
  {
    public ConnectMeetingRequestCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.MeetingRequestId).NotEmpty();

      RuleFor(x => x.Date).NotEmpty()
        .Must(x => x >= DateTimeOffset.UtcNow.AddDays(-1))
        .WithMessage("'Date' must show the future.");

      RuleFor(x => x.ActivityId).NotEmpty();
    }
  }
}
