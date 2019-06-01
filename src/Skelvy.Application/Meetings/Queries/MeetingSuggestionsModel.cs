using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingSuggestionsModel
  {
    public MeetingSuggestionsModel(IList<MeetingRequestDto> meetingRequests, IList<MeetingDto> meetings)
    {
      MeetingRequests = meetingRequests;
      Meetings = meetings;
    }

    public IList<MeetingRequestDto> MeetingRequests { get; }
    public IList<MeetingDto> Meetings { get; }
  }
}
