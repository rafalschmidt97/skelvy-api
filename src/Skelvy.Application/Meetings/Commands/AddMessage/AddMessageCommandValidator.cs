using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.AddMessage
{
  public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
  {
    public AddMessageCommandValidator()
    {
      RuleFor(x => x.Text).MaximumLength(500);
      RuleFor(x => x.Text).NotEmpty().When(x => string.IsNullOrEmpty(x.AttachmentUrl));
      RuleFor(x => x.AttachmentUrl).MaximumLength(2048);
      RuleFor(x => x.AttachmentUrl).NotEmpty().When(x => string.IsNullOrEmpty(x.Text));
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();
    }
  }
}
