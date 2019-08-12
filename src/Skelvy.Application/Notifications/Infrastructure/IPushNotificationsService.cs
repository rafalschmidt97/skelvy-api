using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface IPushNotificationsService
  {
    Task BroadcastUserSentMessage(UserSentMessageNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserJoinedGroup(UserJoinedGroupNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserFoundMeeting(UserFoundMeetingNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification, IEnumerable<int> usersId);
    Task BroadcastMeetingAborted(MeetingAbortedNotification notification, IEnumerable<int> usersId);
    Task BroadcastGroupAborted(GroupAbortedNotification notification, IList<int> usersId);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification, IEnumerable<int> usersId);
    Task BroadcastMeetingExpired(MeetingExpiredNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserSentFriendRequest(UserSentFriendRequestNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestNotification notification, IEnumerable<int> usersId);
  }
}
