using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandValidator : AbstractValidator<AddMeetingChatMessageCommand>
  {
    public AddMeetingChatMessageCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Message).NotEmpty().MaximumLength(500);
    }
  }
}
