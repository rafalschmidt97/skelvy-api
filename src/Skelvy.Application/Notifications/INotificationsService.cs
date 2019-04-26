using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Notifications
{
  public interface INotificationsService
  {
    Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IList<int> usersId);
    Task BroadcastUserJoinedMeeting(MeetingUser user, IList<int> usersId);
    Task BroadcastUserFoundMeeting(IList<int> usersId);
    Task BroadcastUserLeftMeeting(MeetingUser user, IList<int> usersId);
    Task BroadcastMeetingRequestExpired(IList<int> usersId);
    Task BroadcastMeetingExpired(IList<int> usersId);
    Task BroadcastUserCreated(User user);
    Task BroadcastUserRemoved(User user);
    Task BroadcastUserDisabled(User user, string reason);
  }
}
