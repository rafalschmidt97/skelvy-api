using System;
using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.UpdateMeeting
{
  public class UpdateMeetingCommandValidator : AbstractValidator<UpdateMeetingCommand>
  {
    public UpdateMeetingCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Date).NotEmpty()
        .Must(x => x >= DateTimeOffset.UtcNow.AddDays(-1))
        .WithMessage("'Date' must show the future.");

      RuleFor(x => x.Latitude).NotEmpty();
      RuleFor(x => x.Longitude).NotEmpty();
      RuleFor(x => x.Size).NotEmpty();
      RuleFor(x => x.ActivityId).NotEmpty();
      RuleFor(x => x.Description).MaximumLength(500);
    }
  }
}
