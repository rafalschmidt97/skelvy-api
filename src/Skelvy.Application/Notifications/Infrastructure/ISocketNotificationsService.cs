using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface ISocketNotificationsService
  {
    Task BroadcastUserSentMessage(UserSentMessageNotification notification);
    Task BroadcastUserJoinedGroup(UserJoinedGroupNotification notification);
    Task BroadcastUserFoundMeeting(UserFoundMeetingNotification notification);
    Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification);
    Task BroadcastUserLeftGroup(UserLeftGroupNotification notification);
    Task BroadcastMeetingAborted(MeetingAbortedNotification notification);
    Task BroadcastGroupAborted(GroupAbortedNotification notification);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification);
    Task BroadcastMeetingExpired(MeetingExpiredNotification notification);
    Task BroadcastUserRemoved(UserRemovedNotification notification);
    Task BroadcastUserDisabled(UserDisabledNotification notification);
    Task BroadcastUserSentFriendRequest(UserSentFriendRequestNotification notification);
    Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestNotification notification);
  }
}
