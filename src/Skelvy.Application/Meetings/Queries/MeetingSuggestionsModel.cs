using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingSuggestionsModel
  {
    public MeetingSuggestionsModel(IList<MeetingRequestWithUserDto> meetingRequests, IList<MeetingWithUsersDto> meetings)
    {
      MeetingRequests = meetingRequests;
      Meetings = meetings;
    }

    public IList<MeetingRequestWithUserDto> MeetingRequests { get; }
    public IList<MeetingWithUsersDto> Meetings { get; }
  }
}
