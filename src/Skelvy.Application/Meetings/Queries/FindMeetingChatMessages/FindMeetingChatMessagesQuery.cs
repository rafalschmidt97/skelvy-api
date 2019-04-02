using System;
using System.Collections.Generic;
using MediatR;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQuery : IRequest<IList<MeetingChatMessageDto>>
  {
    public int UserId { get; set; }
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }
  }
}
