using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.InviteToMeeting
{
  public class InviteToMeetingCommand : ICommand
  {
    public InviteToMeetingCommand(int userId, int invitingUserId, int meetingId)
    {
      UserId = userId;
      InvitingUserId = invitingUserId;
      MeetingId = meetingId;
    }

    [JsonConstructor]
    public InviteToMeetingCommand()
    {
    }

    public int UserId { get; set; }
    public int InvitingUserId { get; set; }
    public int MeetingId { get; set; }
  }
}
