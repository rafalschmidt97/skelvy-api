using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Messages.Infrastructure.Notifications;
using Skelvy.Application.Relations.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface IPushNotificationsService
  {
    Task BroadcastUserSentMessage(UserSentMessageAction action, IEnumerable<int> usersId);
    Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastUserFoundMeeting(UserFoundMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastUserLeftMeeting(UserLeftMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastMeetingAborted(MeetingAbortedAction action, IEnumerable<int> usersId);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action, IEnumerable<int> usersId);
    Task BroadcastMeetingExpired(MeetingExpiredAction action, IEnumerable<int> usersId);
    Task BroadcastUserSentFriendRequest(UserSentFriendRequestAction action, IEnumerable<int> usersId);
    Task BroadcastUserRespondedFriendRequest(UserRespondedFriendRequestAction action, IEnumerable<int> usersId);
  }
}
