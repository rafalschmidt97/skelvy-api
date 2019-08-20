using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.RemoveUserFromMeeting
{
  public class RemoveUserFromMeetingCommand : ICommand
  {
    public RemoveUserFromMeetingCommand(int userId, int meetingId, int removingUserId)
    {
      UserId = userId;
      MeetingId = meetingId;
      RemovingUserId = removingUserId;
    }

    public int UserId { get; set; }
    public int MeetingId { get; set; }
    public int RemovingUserId { get; set; }
  }
}
