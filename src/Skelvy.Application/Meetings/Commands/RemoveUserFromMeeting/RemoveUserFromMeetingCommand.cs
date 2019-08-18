using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.RemoveUserFromMeeting
{
  public class RemoveUserFromMeetingCommand : ICommand
  {
    public RemoveUserFromMeetingCommand(int userId, int meetingId, int removedUserId)
    {
      UserId = userId;
      MeetingId = meetingId;
      RemovedUserId = removedUserId;
    }

    public int UserId { get; set; }
    public int MeetingId { get; set; }
    public int RemovedUserId { get; set; }
  }
}
