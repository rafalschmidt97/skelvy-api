using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.AddUserToMeeting
{
  public class AddUserToMeetingCommand : ICommand
  {
    public AddUserToMeetingCommand(int userId, int meetingId, int addedUserId)
    {
      UserId = userId;
      MeetingId = meetingId;
      AddedUserId = addedUserId;
    }

    public int UserId { get; set; }
    public int MeetingId { get; set; }
    public int AddedUserId { get; set; }
  }
}
