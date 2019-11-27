using System;
using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.AddMeeting
{
  public class AddMeetingCommandValidator : AbstractValidator<AddMeetingCommand>
  {
    public AddMeetingCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.Date).NotEmpty()
        .Must(x => x >= DateTimeOffset.UtcNow.AddDays(-1))
        .WithMessage("'Date' must show the future.");

      RuleFor(x => x.Latitude).NotEmpty();
      RuleFor(x => x.Longitude).NotEmpty();
      RuleFor(x => x.ActivityId).NotEmpty();
      RuleFor(x => x.Size).NotEmpty()
        .Must(x => x > 2)
        .WithMessage("'Size' must be bigger than 2.")
        .Must(x => x < 10)
        .WithMessage("'Size' must be smaller than 10.");
    }
  }
}
