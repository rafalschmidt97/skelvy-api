using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications.Infrastructure
{
  public interface ISocketNotificationsService
  {
    Task BroadcastUserSentMeetingChatMessage(UserSentMessageAction action, IEnumerable<int> usersId);
    Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastUserFoundMeeting(UserFoundMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastUserLeftMeeting(UserLeftMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action, IEnumerable<int> usersId);
    Task BroadcastMeetingExpired(MeetingExpiredAction action, IEnumerable<int> usersId);
    Task BroadcastUserRemoved(UserRemovedAction action, IEnumerable<int> usersId);
    Task BroadcastUserDisabled(UserDisabledAction action, IEnumerable<int> usersId);
  }
}
