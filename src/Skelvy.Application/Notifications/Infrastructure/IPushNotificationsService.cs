using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Groups.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface IPushNotificationsService
  {
    Task BroadcastUserSentMessage(UserSentMessageNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserJoinedMeeting(UserJoinedMeetingNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserFoundMeeting(UserFoundMeetingNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserRemovedFromMeeting(UserRemovedFromMeetingNotification notification, IList<int> usersId);
    Task BroadcastUserLeftGroup(UserLeftGroupNotification notification, IList<int> usersId);
    Task BroadcastMeetingAborted(MeetingAbortedNotification notification, IEnumerable<int> usersId);
    Task BroadcastGroupAborted(GroupAbortedNotification notification, IList<int> usersId);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification, IEnumerable<int> usersId);
    Task BroadcastMeetingExpired(MeetingExpiredNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserSentFriendInvitation(UserSentFriendInvitationNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserRespondedFriendInvitation(UserRespondedFriendInvitationNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserSentMeetingInvitation(UserSentMeetingInvitationNotification notification, IEnumerable<int> usersId);
    Task BroadcastUserRespondedMeetingInvitation(UserRespondedMeetingInvitationNotification notification, IEnumerable<int> usersId);
  }
}
