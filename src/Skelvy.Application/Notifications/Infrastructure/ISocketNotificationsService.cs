using System.Threading.Tasks;
using Skelvy.Application.Groups.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface ISocketNotificationsService
  {
    Task BroadcastUserSentMessage(UserSentMessageNotification notification);
    Task BroadcastUserJoinedMeeting(UserJoinedMeetingNotification notification);
    Task BroadcastUserConnectedToMeeting(UserConnectedToMeetingNotification notification);
    Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification);
    Task BroadcastUserRemovedFromMeeting(UserRemovedFromMeetingNotification notification);
    Task BroadcastUserSelfRemovedFromMeeting(UserRemovedFromMeetingNotification notification);
    Task BroadcastUserLeftGroup(UserLeftGroupNotification notification);
    Task BroadcastMeetingAborted(MeetingAbortedNotification notification);
    Task BroadcastMeetingUpdated(MeetingUpdatedNotification notification);
    Task BroadcastMeetingUserRoleUpdated(MeetingUserRoleUpdatedNotification notification);
    Task BroadcastGroupAborted(GroupAbortedNotification notification);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification);
    Task BroadcastMeetingExpired(MeetingExpiredNotification notification);
    Task BroadcastUserRemoved(UserRemovedNotification notification);
    Task BroadcastUserDisabled(UserDisabledNotification notification);
    Task BroadcastUserSentFriendInvitation(UserSentFriendInvitationNotification notification);
    Task BroadcastUserRespondedFriendInvitation(UserRespondedFriendInvitationNotification notification);
    Task BroadcastFriendRemoved(FriendRemovedNotification notification);
    Task BroadcastUserSentMeetingInvitation(UserSentMeetingInvitationNotification notification);
    Task BroadcastUserRespondedMeetingInvitation(UserRespondedMeetingInvitationNotification notification);
  }
}
