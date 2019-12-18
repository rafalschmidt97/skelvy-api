using System.Collections.Generic;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Relations.Queries;

namespace Skelvy.Application.Users.Queries
{
  public class SyncModel
  {
    public SyncModel(IList<MeetingRequestDto> requests, IList<MeetingDto> meetings, IList<GroupDto> groups, IList<FriendInvitationsDto> friendInvitations, IList<SelfMeetingInvitationDto> meetingInvitations)
    {
      Requests = requests;
      Meetings = meetings;
      Groups = groups;
      FriendInvitations = friendInvitations;
      MeetingInvitations = meetingInvitations;
    }

    public IList<MeetingRequestDto> Requests { get; }
    public IList<MeetingDto> Meetings { get; }
    public IList<GroupDto> Groups { get; }
    public IList<FriendInvitationsDto> FriendInvitations { get; }
    public IList<SelfMeetingInvitationDto> MeetingInvitations { get; }
  }
}
