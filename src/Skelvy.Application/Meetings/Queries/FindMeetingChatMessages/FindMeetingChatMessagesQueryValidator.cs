using FluentValidation;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQueryValidator : AbstractValidator<FindMeetingChatMessagesQuery>
  {
    public FindMeetingChatMessagesQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Page).NotEmpty();
    }
  }
}
