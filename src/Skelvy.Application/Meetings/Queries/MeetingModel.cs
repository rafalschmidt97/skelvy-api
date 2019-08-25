using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingModel
  {
    public MeetingModel(IList<MeetingDto> meetings, IList<GroupDto> groups)
    {
      Meetings = meetings;
      Groups = groups;
    }

    public IList<MeetingDto> Meetings { get; }
    public IList<GroupDto> Groups { get; }
  }
}
