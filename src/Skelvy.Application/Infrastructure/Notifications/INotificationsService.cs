using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Infrastructure.Notifications
{
  public interface INotificationsService
  {
    Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IList<int> userIds);
    Task BroadcastUserJoinedMeeting(MeetingUser user, IList<int> userIds);
    Task BroadcastUserFoundMeeting(IList<int> userIds);
    Task BroadcastUserLeftMeeting(MeetingUser user, IList<int> userIds);
    Task BroadcastMeetingRequestExpired(IList<int> userIds);
    Task BroadcastMeetingExpired(IList<int> userIds);
    Task BroadcastUserCreated(User user);
    Task BroadcastUserRemoved(User user);
    Task BroadcastUserDisabled(User user, string reason);
  }
}
