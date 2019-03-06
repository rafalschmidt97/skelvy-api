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
        .Must(x => x <= DateTime.Now.Date)
        .WithMessage("'FromDate' must not show the future.");
      RuleFor(x => x.ToDate).NotEmpty()
        .Unless(x => x.ToDate.Date >= x.FromDate.Date)
        .WithMessage("'ToDate' must be after 'FromDate'.")
        .Unless(x => (x.ToDate.Date - x.FromDate.Date).TotalDays <= 7)
        .WithMessage("Range from 'FromDate' to 'ToDate' is too wide");
    }
  }
}
