using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Notifications
{
  public interface INotificationsService
  {
    Task BroadcastUserSentMessage(UserSentMessageAction action, IEnumerable<int> usersId);
    Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastUserFoundMeeting(UserFoundMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastUserLeftMeeting(UserLeftMeetingAction action, IEnumerable<int> usersId);
    Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action, IEnumerable<int> usersId);
    Task BroadcastMeetingExpired(MeetingExpiredAction action, IEnumerable<int> usersId);
    Task BroadcastUserCreated(UserCreatedAction action);
    Task BroadcastUserRemoved(UserRemovedAction action);
    Task BroadcastUserDisabled(UserDisabledAction action);
  }
}
