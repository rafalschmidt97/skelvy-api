using System.Threading.Tasks;
using Skelvy.Application.Groups.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications
{
  public interface INotificationsService
  {
    Task BroadcastUserSentMessage(UserSentMessageNotification notification);
    Task BroadcastUserJoinedMeeting(UserJoinedMeetingNotification notification);
    Task BroadcastUserConnectedToMeeting(UserConnectedToMeetingNotification notification);
    Task BroadcastUserLeftMeeting(UserLeftMeetingNotification notification);
    Task BroadcastUserRemovedFromGroup(UserRemovedFromGroupNotification notification);
    Task BroadcastUserSelfRemovedFromGroup(UserRemovedFromGroupNotification notification);
    Task BroadcastUserLeftGroup(UserLeftGroupNotification notification);
    Task BroadcastMeetingAborted(MeetingAbortedNotification notification);
    Task BroadcastMeetingUpdated(MeetingUpdatedNotification notification);
    Task BroadcastGroupUpdated(GroupUpdatedNotification notification);
    Task BroadcastGroupUserRoleUpdated(GroupUserRoleUpdatedNotification notification);
    Task BroadcastGroupAborted(GroupAbortedNotification notification);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredNotification notification);
    Task BroadcastMeetingExpired(MeetingExpiredNotification notification);
    Task BroadcastUserCreated(UserCreatedNotification notification);
    Task BroadcastUserRemoved(UserRemovedNotification notification);
    Task BroadcastUserDisabled(UserDisabledNotification notification);
    Task BroadcastUserSentFriendInvitation(UserSentFriendInvitationNotification notification);
    Task BroadcastUserRespondedFriendInvitation(UserRespondedFriendInvitationNotification notification);
    Task BroadcastFriendRemoved(FriendRemovedNotification notification);
    Task BroadcastUserSentMeetingInvitation(UserSentMeetingInvitationNotification notification);
    Task BroadcastUserRespondedMeetingInvitation(UserRespondedMeetingInvitationNotification notification);
  }
}
