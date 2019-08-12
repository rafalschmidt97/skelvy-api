using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Queries
{
  public class GroupsModel
  {
    public GroupsModel(IList<MeetingRequestDto> requests, IList<MeetingDto> meetings, IList<GroupDto> groups)
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
