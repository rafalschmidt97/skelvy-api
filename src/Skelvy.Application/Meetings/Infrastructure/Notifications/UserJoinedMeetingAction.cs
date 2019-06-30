namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserJoinedMeetingAction
  {
    public UserJoinedMeetingAction(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; private set; }
  }
}
