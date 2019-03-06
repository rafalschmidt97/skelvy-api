using System;
using FluentValidation;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQueryValidator : AbstractValidator<FindMeetingChatMessagesQuery>
  {
    public FindMeetingChatMessagesQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.FromDate).NotEmpty()
        .Must(x => x <= DateTimeOffset.Now)
        .WithMessage("'FromDate' must not show the future.");
      RuleFor(x => x.ToDate).NotEmpty()
        .Unless(x => x.ToDate >= x.FromDate)
        .WithMessage("'ToDate' must be after 'FromDate'.")
        .Unless(x => (x.ToDate - x.FromDate).TotalDays <= 7)
        .WithMessage("Range from 'FromDate' to 'ToDate' is too wide");
    }
  }
}
