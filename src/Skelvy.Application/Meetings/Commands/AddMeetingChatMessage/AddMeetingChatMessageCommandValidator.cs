using System;
using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandValidator : AbstractValidator<AddMeetingChatMessageCommand>
  {
    public AddMeetingChatMessageCommandValidator()
    {
      RuleFor(x => x.Date).NotEmpty()
        .Must(x => x >= DateTimeOffset.UtcNow.AddHours(-1) && x <= DateTimeOffset.UtcNow.AddHours(1))
        .WithMessage("'Date' must show the present.");

      RuleFor(x => x.Message).NotEmpty().MaximumLength(500);
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
