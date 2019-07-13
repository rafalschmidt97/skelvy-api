using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandValidator : AbstractValidator<AddMeetingChatMessageCommand>
  {
    public AddMeetingChatMessageCommandValidator()
    {
      RuleFor(x => x.Message).MaximumLength(500);
      RuleFor(x => x.Message).NotEmpty().When(x => string.IsNullOrEmpty(x.AttachmentUrl));
      RuleFor(x => x.AttachmentUrl).MaximumLength(2048);
      RuleFor(x => x.AttachmentUrl).NotEmpty().When(x => string.IsNullOrEmpty(x.Message));
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
