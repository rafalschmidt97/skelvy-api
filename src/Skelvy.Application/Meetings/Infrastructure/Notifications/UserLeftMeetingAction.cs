namespace Skelvy.Application.Meetings.Infrastructure.Notifications
{
  public class UserLeftMeetingAction
  {
    public UserLeftMeetingAction(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; private set; }
  }
}
