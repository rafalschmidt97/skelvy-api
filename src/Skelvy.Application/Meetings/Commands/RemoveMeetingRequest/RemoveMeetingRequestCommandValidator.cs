using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommandValidator : AbstractValidator<RemoveMeetingRequestCommand>
  {
    public RemoveMeetingRequestCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
