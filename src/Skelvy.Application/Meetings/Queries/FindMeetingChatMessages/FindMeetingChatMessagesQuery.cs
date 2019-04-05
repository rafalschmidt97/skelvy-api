using System.Collections.Generic;
using MediatR;

namespace Skelvy.Application.Meetings.Queries.FindMeetingChatMessages
{
  public class FindMeetingChatMessagesQuery : IRequest<IList<MeetingChatMessageDto>>
  {
    public int UserId { get; set; }
    public int Page { get; set; }
  }
}
