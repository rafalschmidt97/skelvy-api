using FluentValidation;
using Skelvy.Application.Meetings.Queries.FindMeetingChatMessages;

namespace Skelvy.Application.Users.Queries.FindBlockedUsers
{
  public class FindBlockedUsersQueryValidator : AbstractValidator<FindMeetingChatMessagesQuery>
  {
    public FindBlockedUsersQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Page).NotEmpty();
    }
  }
}
