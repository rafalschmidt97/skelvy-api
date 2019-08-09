using FluentValidation;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Application.Meetings.Commands.AddMessage
{
  public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
  {
    public AddMessageCommandValidator()
    {
      RuleFor(x => x.Type).NotEmpty().MaximumLength(15)
        .Must(x => x == MessageTypes.Action || x == MessageTypes.Response)
        .WithMessage($"'Type' must be {MessageTypes.Action} / {MessageTypes.Response}");

      When(x => x.Type == MessageTypes.Response, () =>
      {
        RuleFor(x => x.Text).MaximumLength(500);
        RuleFor(x => x.Text).NotEmpty().When(x => string.IsNullOrEmpty(x.AttachmentUrl));
        RuleFor(x => x.AttachmentUrl).MaximumLength(2048);
        RuleFor(x => x.AttachmentUrl).NotEmpty().When(x => string.IsNullOrEmpty(x.Text));
        RuleFor(x => x.Action).Empty();
      });

      When(x => x.Type == MessageTypes.Action, () =>
      {
        RuleFor(x => x.Text).Empty();
        RuleFor(x => x.AttachmentUrl).Empty();
        RuleFor(x => x.Action).NotEmpty().MaximumLength(15);
        RuleFor(x => x.Action).Must(x =>
            x == MessageActionTypes.Seen || x == MessageActionTypes.TypingOn || x == MessageActionTypes.TypingOff)
          .WithMessage(
            $"'Action' must be {MessageActionTypes.Seen} / {MessageActionTypes.TypingOn} / {MessageActionTypes.TypingOff}");
      });

      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();
    }
  }
}
