using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQuery : IQuery<IList<MeetingChatMessageDto>>
  {
    public int UserId { get; set; }
    public int Page { get; set; }
  }
}
