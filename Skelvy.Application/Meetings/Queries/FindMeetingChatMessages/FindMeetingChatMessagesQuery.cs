using System;
using System.Collections.Generic;
using MediatR;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQuery : IRequest<ICollection<MeetingChatMessageDto>>
  {
    public int UserId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
  }
}
