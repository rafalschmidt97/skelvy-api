using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingViewModel
  {
    public string Status { get; set; }
    public MeetingDto Meeting { get; set; }
    public IList<MeetingChatMessageDto> MeetingMessages { get; set; }
    public MeetingRequestDto Request { get; set; }
  }
}
