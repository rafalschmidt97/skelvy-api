using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.DeleteMeetingRequest
{
  public class DeleteMeetingRequestCommandValidator : AbstractValidator<DeleteMeetingRequestCommand>
  {
    public DeleteMeetingRequestCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
