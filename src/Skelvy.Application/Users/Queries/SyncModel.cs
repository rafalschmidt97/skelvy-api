using System.Collections.Generic;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Users.Queries
{
  public class SyncModel
  {
    public SyncModel(IList<MeetingRequestDto> requests, IList<MeetingDto> meetings, IList<GroupDto> groups)
    {
      Requests = requests;
      Meetings = meetings;
      Groups = groups;
    }

    public IList<MeetingRequestDto> Requests { get; }
    public IList<MeetingDto> Meetings { get; }
    public IList<GroupDto> Groups { get; }
  }
}
