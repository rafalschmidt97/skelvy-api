using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.UsersConnectedToMeeting
{
  public class UsersConnectedToMeetingEvent : IEvent
  {
    public UsersConnectedToMeetingEvent(int user1Id, int user2Id, int meetingId)
    {
      User1Id = user1Id;
      User2Id = user2Id;
      MeetingId = meetingId;
    }

    public int User1Id { get; private set; }
    public int User2Id { get; private set; }
    public int MeetingId { get; private set; }
  }
}
