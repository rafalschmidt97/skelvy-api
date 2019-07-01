using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQuery : IQuery<IList<MeetingChatMessageDto>>
  {
    public FindMeetingChatMessagesQuery(int userId, DateTimeOffset beforeDate)
    {
      UserId = userId;
      BeforeDate = beforeDate;
    }

    public FindMeetingChatMessagesQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public DateTimeOffset BeforeDate { get; set; }
  }
}
