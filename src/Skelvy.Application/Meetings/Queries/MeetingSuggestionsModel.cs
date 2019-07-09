using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingSuggestionsModel
  {
    public MeetingSuggestionsModel(IList<MeetingRequestWithUserDto> meetingRequests, IList<MeetingDto> meetings)
    {
      MeetingRequests = meetingRequests;
      Meetings = meetings;
    }

    public IList<MeetingRequestWithUserDto> MeetingRequests { get; }
    public IList<MeetingDto> Meetings { get; }
  }
}
