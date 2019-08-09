using FluentValidation;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Application.Messages.Commands.AddMessage
{
  public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
  {
    public AddMessageCommandValidator()
    {
      RuleFor(x => x.Type).NotEmpty().MaximumLength(15)
        .Must(x => x == MessageType.Action || x == MessageType.Response)
        .WithMessage($"'Type' must be {MessageType.Action} / {MessageType.Response}");

      When(x => x.Type == MessageType.Response, () =>
      {
        RuleFor(x => x.Text).MaximumLength(500);
        RuleFor(x => x.Text).NotEmpty().When(x => string.IsNullOrEmpty(x.AttachmentUrl));
        RuleFor(x => x.AttachmentUrl).MaximumLength(2048);
        RuleFor(x => x.AttachmentUrl).NotEmpty().When(x => string.IsNullOrEmpty(x.Text));
        RuleFor(x => x.Action).Empty();
      });

      When(x => x.Type == MessageType.Action, () =>
      {
        RuleFor(x => x.Text).Empty();
        RuleFor(x => x.AttachmentUrl).Empty();
        RuleFor(x => x.Action).NotEmpty().MaximumLength(15);
        RuleFor(x => x.Action).Must(x =>
            x == MessageActionType.Seen || x == MessageActionType.TypingOn || x == MessageActionType.TypingOff)
          .WithMessage(
            $"'Action' must be {MessageActionType.Seen} / {MessageActionType.TypingOn} / {MessageActionType.TypingOff}");
      });

      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();
    }
  }
}
