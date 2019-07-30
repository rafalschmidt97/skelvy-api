using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface ISocketNotificationsService
  {
    Task BroadcastUserSentMessage(UserSentMessageAction action);
    Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action);
    Task BroadcastUserFoundMeeting(UserFoundMeetingAction action);
    Task BroadcastUserLeftMeeting(UserLeftMeetingAction action);
    Task BroadcastMeetingAborted(MeetingAbortedAction action);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action);
    Task BroadcastMeetingExpired(MeetingExpiredAction action);
    Task BroadcastUserRemoved(UserRemovedAction action);
    Task BroadcastUserDisabled(UserDisabledAction action);
  }
}
