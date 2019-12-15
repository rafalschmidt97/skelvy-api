using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Events.MeetingUserRoleUpdated
{
  public class MeetingUserRoleUpdatedEvent : IEvent
  {
    public MeetingUserRoleUpdatedEvent(int userId, int meetingId, int groupId, int updatedUserId, string role)
    {
      UserId = userId;
      MeetingId = meetingId;
      GroupId = groupId;
      UpdatedUserId = updatedUserId;
      Role = role;
    }

    public int UserId { get; private set; }
    public int MeetingId { get; private set; }
    public int GroupId { get; private set; }
    public int UpdatedUserId { get; private set; }
    public string Role { get; private set; }
  }
}
